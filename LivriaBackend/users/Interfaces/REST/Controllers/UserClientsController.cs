using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.ComponentModel; 
using System.Linq;
using LivriaBackend.commerce.Interfaces.REST.Resources; 
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using LivriaBackend.users.Domain.Model.Aggregates;
using LivriaBackend.users.Domain.Model.Commands;
using LivriaBackend.users.Domain.Model.Queries;
using LivriaBackend.users.Domain.Model.Services;
using LivriaBackend.users.Interfaces.REST.Resources;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace LivriaBackend.users.Interfaces.REST.Controllers
{
    /// <summary>
    /// Controlador RESTful para la gestión de clientes de usuario.
    /// Expone endpoints para operaciones CRUD de <see cref="UserClient"/>,
    /// gestión de libros favoritos, libros excluidos y actualización de suscripciones.
    /// </summary>
    [ApiController]
    [Route("api/v1/userclients")]
    [Produces(MediaTypeNames.Application.Json)] 
    public class UserClientsController : ControllerBase
    {
        private readonly IUserClientCommandService _userClientCommandService;
        private readonly IUserClientQueryService _userClientQueryService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserClientsController"/>.
        /// </summary>
        /// <param name="userClientCommandService">El servicio de comandos para clientes de usuario.</param>
        /// <param name="userClientQueryService">El servicio de consultas para clientes de usuario.</param>
        /// <param name="mapper">La instancia de AutoMapper para el mapeo entre objetos.</param>
        public UserClientsController(
            IUserClientCommandService userClientCommandService,
            IUserClientQueryService userClientQueryService,
            IMapper mapper,
            IMediator mediator)
        {
            _userClientCommandService = userClientCommandService;
            _userClientQueryService = userClientQueryService;
            _mapper = mapper;
            _mediator = mediator;
        }
        
        /// <summary>
        /// Crea un nuevo cliente de usuario en el sistema con una suscripción por defecto 'freeplan'.
        /// </summary>
        /// <param name="resource">Los datos para la creación del nuevo cliente de usuario en formato <see cref="CreateUserClientResource"/>.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserClientResource"/> recién creado
        /// si la operación fue exitosa (código 201 Created).
        /// Retorna 400 Bad Request si los datos de entrada son inválidos o si ya existe un usuario con el mismo nombre de usuario/email.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary= "Añadir nuevo cliente.",
            Description= "Crea un nuevo cliente en el sistema con suscripción 'freeplan' por defecto."
            )]
        [ProducesResponseType(typeof(UserClientResource), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserClientResource>> CreateUserClient([FromBody] CreateUserClientResource resource)
        {
            // Creamos el RegisterUserClientCompositeCommand con todos los datos, incluyendo la contraseña
            var command = new RegisterUserClientCompositeCommand(
                resource.Username,
                resource.Password,
                resource.Display,
                resource.Email,
                resource.Icon,
                resource.Phrase
            );
            
            try
            {
                var userClientResource = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(GetUserClientById), new { id = userClientResource.Id }, userClientResource);
            }
            
           
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            catch (LivriaBackend.shared.Domain.Exceptions.DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred while creating the user client." });
            }
        }

        /// <summary>
        /// Obtiene todos los clientes de usuario del sistema.
        /// </summary>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="UserClientResource"/>
        /// si la operación fue exitosa (código 200 OK).
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(
            Summary= "Obtener los datos de todos los clientes.",
            Description= "Te muestra los datos de los clientes."
            
        )]
        [ProducesResponseType(typeof(IEnumerable<UserClientResource>), 200)]
        public async Task<ActionResult<IEnumerable<UserClientResource>>> GetAllUserClients()
        {
            var query = new GetAllUserClientQuery(); 
            var userClients = await _userClientQueryService.Handle(query);
            var userClientResources = _mapper.Map<IEnumerable<UserClientResource>>(userClients);
            return Ok(userClientResources);
        }

        /// <summary>
        /// Obtiene los datos de un cliente de usuario específico por su ID.
        /// </summary>
        /// <param name="id">El identificador único del cliente de usuario a buscar.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserClientResource"/>
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 404 Not Found si el cliente de usuario no se encuentra.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary= "Obtener los datos de un usuario en específico.",
            Description= "Te muestra los datos del usuario que buscaste."
        )]
        [ProducesResponseType(typeof(UserClientResource), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserClientResource>> GetUserClientById(int id)
        {
            var query = new GetUserClientByIdQuery(id);
            var userClient = await _userClientQueryService.Handle(query);
            if (userClient == null)
            {
                return NotFound(new { message = $"UserClient with ID {id} not found." }); 
            }
            var userClientResource = _mapper.Map<UserClientResource>(userClient);
            return Ok(userClientResource);
        }

        /// <summary>
        /// Actualiza los datos de un cliente de usuario existente.
        /// </summary>
        /// <param name="id">El identificador único del cliente de usuario a actualizar.</param>
        /// <param name="resource">Los datos actualizados del cliente de usuario en formato <see cref="UpdateUserClientResource"/>.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserClientResource"/> actualizado
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 400 Bad Request si los datos de entrada son inválidos.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [Authorize(Roles = "UserClient,Admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary= "Actualizar los datos de un UserClient existente.",
            Description= "Te permite modificar los datos de un UserClient previamente creado."
        )]
        [ProducesResponseType(typeof(UserClientResource), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserClientResource>> UpdateUserClient(int id, [FromBody] UpdateUserClientResource resource)
        {
            var command = new UpdateUserClientCommand(
                id, 
                resource.Display,
                resource.Email,
                resource.Icon,
                resource.Phrase
                );

            try
            {
                var updatedUserClient = await _userClientCommandService.Handle(command);
                var userClientResource = _mapper.Map<UserClientResource>(updatedUserClient);
                return Ok(userClientResource);
            }
            
            catch (ArgumentException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
            
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred while updating the user client." });
            }
        }
        
        /// <summary>
        /// Actualiza el plan de suscripción de un cliente de usuario.
        /// </summary>
        /// <param name="id">El identificador único del cliente de usuario.</param>
        /// <param name="resource">El nuevo plan de suscripción en formato <see cref="UpdateUserClientSubscriptionResource"/>.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserClientResource"/> actualizado
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 400 Bad Request si el plan de suscripción es inválido.
        /// Retorna 404 Not Found si el cliente de usuario no se encuentra.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [Authorize(Roles = "UserClient,Admin")]
        [HttpPut("{id}/subscription")]
        [SwaggerOperation(
            Summary = "Actualizar el plan de suscripción de un cliente de usuario.",
            Description = "Permite cambiar la suscripción del usuario a 'freeplan' o 'communityplan'."
        )]
        [ProducesResponseType(typeof(UserClientResource), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserClientResource>> UpdateUserClientSubscription(int id, [FromBody] UpdateUserClientSubscriptionResource resource)
        {
            var command = new UpdateUserClientSubscriptionCommand(id, resource.NewSubscriptionPlan);
            try
            {
                var userClient = await _userClientCommandService.Handle(command);
                
                if (userClient == null) 
                {
                    return NotFound(new { message = $"UserClient with ID {id} not found." });
                }
                var userClientResource = _mapper.Map<UserClientResource>(userClient);
                return Ok(userClientResource);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception) 
            {
                return StatusCode(500, new { message = "An unexpected error occurred while updating the user client subscription." });
            }
        }

        /// <summary>
        /// Elimina un cliente de usuario del sistema.
        /// </summary>
        /// <param name="id">El identificador único del cliente de usuario a eliminar.</param>
        /// <returns>
        /// Una acción de resultado HTTP sin contenido (código 204 No Content) si la eliminación fue exitosa.
        /// Retorna 404 Not Found si el cliente de usuario no se encuentra.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [Authorize(Roles = "UserClient,Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary= "Eliminar un UserClient previamente creado.",
            Description= "Elimina un UserClient del sistema."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUserClient(int id)
        {
            var command = new DeleteUserClientCommand(id);
            try
            {
                bool removed = await _userClientCommandService.Handle(command); 
                if (!removed)
                {
                    return NotFound(new { message = $"UserClient with ID {id} not found." });
                }
                return NoContent();
            }
            
            catch (ArgumentException ex) 
            {
                
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred while deleting the user client." });
            }
        }
        
        /// <summary>
        /// Agrega un libro a la lista de favoritos de un cliente de usuario.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <param name="bookId">El identificador único del libro a añadir como favorito.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserClientResource"/> actualizado
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 404 Not Found si el cliente de usuario o el libro no se encuentran.
        /// Retorna 409 Conflict si el libro ya está en los favoritos del usuario.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [Authorize(Roles = "UserClient,Admin")]
        [HttpPost("{userClientId}/favorites/{bookId}")]
        [SwaggerOperation(
            Summary= "Agregar un libro existente como favorito.",
            Description= "Agrega un libro existente como favorito en el sistema."
         )]
        [ProducesResponseType(typeof(UserClientResource), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserClientResource>> AddBookToFavorites(int userClientId, int bookId)
        {
            var command = new AddFavoriteBookCommand(userClientId, bookId);
            try
            {
                var userClient = await _userClientCommandService.Handle(command);
                var userClientResource = _mapper.Map<UserClientResource>(userClient);
                return Ok(userClientResource); 
            }
            catch (ArgumentException ex) 
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred: " + ex.Message });
            }
        }
        
        /// <summary>
        /// Elimina un libro de la lista de favoritos de un cliente de usuario.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <param name="bookId">El identificador único del libro a eliminar de favoritos.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserClientResource"/> actualizado
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 404 Not Found si el cliente de usuario o el libro no se encuentran.
        /// Retorna 409 Conflict si el libro no está en los favoritos del usuario.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [Authorize(Roles = "UserClient,Admin")]
        [HttpDelete("{userClientId}/favorites/{bookId}")]
        [SwaggerOperation(
            Summary= "Eliminar un libro favorito de un UserClient previamente creado.",
            Description= "Elimina un libro favorito de un UserClient del sistema."
        )]
        [ProducesResponseType(typeof(UserClientResource), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserClientResource>> RemoveBookFromFavorites(int userClientId, int bookId)
        {
            var command = new RemoveFavoriteBookCommand(userClientId, bookId);
            try
            {
                var userClient = await _userClientCommandService.Handle(command);
                var userClientResource = _mapper.Map<UserClientResource>(userClient);
                return Ok(userClientResource); 
            }
            catch (ArgumentException ex) 
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred: " + ex.Message });
            }
        }
        
        /// <summary>
        /// Obtiene todos los libros favoritos de un cliente de usuario específico.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="BookResource"/>
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 404 Not Found si el cliente de usuario no se encuentra.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("{userClientId}/favorites")]
        [SwaggerOperation(
            Summary= "Obtener los datos de los favoritos que le pertenecen a un usuario en específico.",
            Description= "Te muestra los datos de los favoritos por medio del id de un usuario en específico."
        )]
        [ProducesResponseType(typeof(IEnumerable<BookResource>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<BookResource>>> GetUserFavoriteBooks(int userClientId)
        {
            var query = new GetUserClientByIdQuery(userClientId); 
            var userClient = await _userClientQueryService.Handle(query);

            if (userClient == null)
            {
                return NotFound(new { message = $"UserClient with ID {userClientId} not found." });
            }

            var favoriteBookResources = _mapper.Map<IEnumerable<BookResource>>(userClient.FavoriteBooks);
            return Ok(favoriteBookResources);
        }
        
        /// <summary>
        /// Agrega un libro a la lista de exclusión de un cliente de usuario.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <param name="bookId">El identificador único del libro a añadir a la lista de exclusión.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserClientResource"/> actualizado
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 404 Not Found si el cliente de usuario o el libro no se encuentran.
        /// Retorna 409 Conflict si el libro ya está en la lista de exclusión del usuario.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [Authorize(Roles = "UserClient,Admin")]
        [HttpPost("{userClientId}/exclusions/{bookId}")]
        [SwaggerOperation(
            Summary= "Agregar un libro existente a la lista de exclusión.",
            Description= "Agrega un libro existente que el usuario no desea ver o recomendar en el sistema."
         )]
        [ProducesResponseType(typeof(UserClientResource), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserClientResource>> AddBookToExclusions(int userClientId, int bookId)
        {
            var command = new AddExclusionBookCommand(userClientId, bookId);
            try
            {
                var userClient = await _userClientCommandService.Handle(command);
                var userClientResource = _mapper.Map<UserClientResource>(userClient);
                return Ok(userClientResource); 
            }
            catch (ArgumentException ex) 
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred: " + ex.Message });
            }
        }
        
        /// <summary>
        /// Elimina un libro de la lista de exclusión de un cliente de usuario.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <param name="bookId">El identificador único del libro a eliminar de la lista de exclusión.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserClientResource"/> actualizado
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 404 Not Found si el cliente de usuario o el libro no se encuentran.
        /// Retorna 409 Conflict si el libro no está en la lista de exclusión del usuario.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [Authorize(Roles = "UserClient,Admin")]
        [HttpDelete("{userClientId}/exclusions/{bookId}")]
        [SwaggerOperation(
            Summary= "Eliminar un libro de la lista de exclusión de un UserClient.",
            Description= "Elimina un libro de la lista de exclusión del sistema."
        )]
        [ProducesResponseType(typeof(UserClientResource), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserClientResource>> RemoveBookFromExclusions(int userClientId, int bookId)
        {
            var command = new RemoveExclusionBookCommand(userClientId, bookId);
            try
            {
                var userClient = await _userClientCommandService.Handle(command);
                var userClientResource = _mapper.Map<UserClientResource>(userClient);
                return Ok(userClientResource); 
            }
            catch (ArgumentException ex) 
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred: " + ex.Message });
            }
        }
        
        /// <summary>
        /// Obtiene todos los libros excluidos de un cliente de usuario específico.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="BookResource"/>
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 404 Not Found si el cliente de usuario no se encuentra.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("{userClientId}/exclusions")]
        [SwaggerOperation(
            Summary= "Obtener los datos de los libros excluidos que le pertenecen a un usuario en específico.",
            Description= "Te muestra los datos de los libros excluidos por medio del id de un usuario en específico."
        )]
        [ProducesResponseType(typeof(IEnumerable<BookResource>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<BookResource>>> GetUserExclusionBooks(int userClientId)
        {
            var query = new GetUserClientByIdQuery(userClientId); 
            var userClient = await _userClientQueryService.Handle(query);

            if (userClient == null)
            {
                return NotFound(new { message = $"UserClient with ID {userClientId} not found." });
            }

            var exclusionBookResources = _mapper.Map<IEnumerable<BookResource>>(userClient.ExclusionBooks);
            return Ok(exclusionBookResources);
        }
        
        /// <summary>
        /// Verifica si un cliente de usuario tiene el plan de suscripción 'communityplan'.
        /// Este endpoint es crucial para validar si un usuario puede crear o unirse a comunidades.
        /// </summary>
        /// <param name="id">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un objeto JSON con el campo 'hasCommunityPlan' (bool) (código 200 OK).
        /// Retorna 404 Not Found si el cliente de usuario no se encuentra.
        /// </returns>
        [AllowAnonymous] // Permitimos la consulta de estado de suscripción
        [HttpGet("{id}/has-community-plan")]
        [SwaggerOperation(
            Summary= "Verificar el plan de suscripción a comunidades.",
            Description= "Comprueba si un cliente de usuario tiene el plan de suscripción 'communityplan'." 
            )]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<object>> CheckUserHasCommunityPlan(int id)
        { 
            try 
            {
                var hasPlan = await _userClientQueryService.HasCommunityPlanAsync(id);
                
                if (!hasPlan)
                {
                    var checkExistence = await _userClientQueryService.Handle(new GetUserClientByIdQuery(id));
                    if (checkExistence == null)
                    {
                        return NotFound(new { message = $"UserClient with ID {id} not found." });
                    }
                }

                return Ok(new { hasCommunityPlan = hasPlan });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred while checking the subscription plan." });
            }
        }
    }
}