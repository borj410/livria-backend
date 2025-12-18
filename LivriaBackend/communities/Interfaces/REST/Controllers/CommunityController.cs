using AutoMapper;
using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Services;
using LivriaBackend.communities.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; 

namespace LivriaBackend.communities.Interfaces.REST.Controllers
{
    /// <summary>
    /// Controlador RESTful para gestionar las operaciones relacionadas con las comunidades.
    /// </summary>
    [Authorize(Roles = "UserClient,Admin")]
    [ApiController]
    [Route("api/v1/communities")]
    [Produces(MediaTypeNames.Application.Json)] 
    public class CommunitiesController : ControllerBase
    {
        private readonly ICommunityCommandService _communityCommandService;
        private readonly ICommunityQueryService _communityQueryService;
        private readonly IUserCommunityCommandService _userCommunityCommandService; 
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CommunitiesController"/>.
        /// </summary>
        /// <param name="communityCommandService">El servicio de comandos de comunidades.</param>
        /// <param name="communityQueryService">El servicio de consulta de comunidades.</param>
        /// <param name="userCommunityCommandService">El servicio de comandos para relaciones usuario-comunidad.</param>
        /// <param name="mapper">La instancia de AutoMapper para la transformación de objetos.</param>
        public CommunitiesController(
            ICommunityCommandService communityCommandService,
            ICommunityQueryService communityQueryService,
            IUserCommunityCommandService userCommunityCommandService, 
            IMapper mapper)
        {
            _communityCommandService = communityCommandService;
            _communityQueryService = communityQueryService;
            _userCommunityCommandService = userCommunityCommandService; 
            _mapper = mapper;
        }

        /// <summary>
        /// Crea una nueva comunidad en el sistema.
        /// </summary>
        /// <param name="resource">El recurso que contiene los datos de la comunidad a crear.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="CommunityResource"/> de la comunidad creada
        /// con un código 201 CreatedAtAction si la operación es exitosa.
        /// Retorna BadRequest (400) si la comunidad no pudo ser creada debido a datos inválidos.
        /// </returns>
        [HttpPost]
        [SwaggerOperation(
            Summary= "Crear una nueva comunidad.",
            Description= "Crea una nueva comunidad en el sistema."
        )]
        [ProducesResponseType(typeof(CommunityResource), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CommunityResource>> CreateCommunity([FromBody] CreateCommunityResource resource)
        {
            var command = _mapper.Map<CreateCommunityResource, CreateCommunityCommand>(resource);
            var community = await _communityCommandService.Handle(command);

            if (community == null)
            {
                return BadRequest("Could not create community. Check provided data.");
            }

            var communityResource = _mapper.Map<Community, CommunityResource>(community);
            return CreatedAtAction(nameof(GetCommunityById), new { id = communityResource.Id }, communityResource);
        }

        /// <summary>
        /// Obtiene los datos de todas las comunidades disponibles en el sistema.
        /// </summary>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="CommunityResource"/>
        /// si la operación es exitosa (código 200 OK). Puede ser una colección vacía si no hay comunidades.
        /// </returns>
        [HttpGet]
        [SwaggerOperation(
            Summary= "Obtener los datos de todas las comunidades.",
            Description= "Te muestra los datos de las comunidades."
            
        )]
        [ProducesResponseType(typeof(IEnumerable<CommunityResource>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CommunityResource>>> GetAllCommunities()
        {
            var query = new GetAllCommunitiesQuery();
            var communities = await _communityQueryService.Handle(query);
            var communityResources = _mapper.Map<IEnumerable<Community>, IEnumerable<CommunityResource>>(communities);
            return Ok(communityResources);
        }

        /// <summary>
        /// Obtiene los datos de una comunidad específica por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la comunidad.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un <see cref="CommunityResource"/> si la comunidad es encontrada (código 200 OK),
        /// o un resultado NotFound (código 404) si la comunidad no existe.
        /// </returns>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary= "Obtener los datos de una comunidad en específico.",
            Description= "Te muestra los datos de la comunidad que buscaste."
        )]
        [ProducesResponseType(typeof(CommunityResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CommunityResource>> GetCommunityById(int id)
        {
            var query = new GetCommunityByIdQuery(id);
            var community = await _communityQueryService.Handle(query);

            if (community == null)
            {
                return NotFound();
            }

            var communityResource = _mapper.Map<Community, CommunityResource>(community);
            return Ok(communityResource);
        }

        /// <summary>
        /// Permite a un cliente de usuario unirse a una comunidad existente.
        /// </summary>
        /// <param name="resource">El recurso que contiene los IDs del cliente de usuario y la comunidad.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserCommunityResource"/> de la membresía creada
        /// con un código 201 CreatedAtAction si la operación es exitosa.
        /// Retorna BadRequest (400) si la unión no es posible (ej. usuario o comunidad no encontrados, suscripción inválida, ya es miembro).
        /// Retorna StatusCode 500 si ocurre un error inesperado.
        /// </returns>
        [HttpPost("join")]
        [SwaggerOperation(
            Summary= "Unirse una comunidad existente.",
            Description= "El userclient puede unirse a una comunidad existente."
        )]
        [ProducesResponseType(typeof(UserCommunityResource), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserCommunityResource>> JoinCommunity([FromBody] JoinCommunityResource resource)
        {
            var command = _mapper.Map<JoinCommunityResource, JoinCommunityCommand>(resource);
            try
            {
                var userCommunity = await _userCommunityCommandService.Handle(command);
                var userCommunityResource = _mapper.Map<UserCommunity, UserCommunityResource>(userCommunity);
                return CreatedAtAction(nameof(JoinCommunity), userCommunityResource);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while trying to join the community.");
            }
        }

        /// <summary>
        /// Permite a un usuario salir de una comunidad.
        /// </summary>
        /// <param name="communityId">El ID de la comunidad de la que el usuario quiere salir.</param>
        /// <param name="userId">El ID del cliente de usuario que quiere salir.</param>
        /// <returns>Un No Content (204) si la operación fue exitosa, o Not Found (404) si la membresía no existía.</returns>
        [HttpDelete("{communityId}/members/{userId}")] // ¡NUEVO ENDPOINT AGREGADO!
        [SwaggerOperation(
            Summary = "Salir de una comunidad.",
            Description = "Permite a un usuario salir de una comunidad específica."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LeaveCommunity(int communityId, int userId)
        {
           
            var command = new LeaveCommunityCommand(userId, communityId);
            try
            {
                var result = await _userCommunityCommandService.Handle(command);

                if (!result)
                {
                    return NotFound($"Membership not found for User ID {userId} in Community ID {communityId}.");
                }

                return NoContent(); 
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Verifica si un cliente de usuario es miembro de una comunidad específica.
        /// </summary>
        /// <param name="communityId">El ID de la comunidad a verificar.</param>
        /// <param name="userId">El ID del cliente de usuario cuya membresía se verifica.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un objeto JSON con el campo 'isMember' (bool) (código 200 OK).
        /// </returns>
        [HttpGet("{communityId}/members/{userId}/is-member")]
        [SwaggerOperation(
            Summary= "Verificar la membresía de un usuario en una comunidad.",
            Description= "Comprueba si un cliente de usuario es actualmente miembro de la comunidad especificada."
        )]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> CheckUserJoined(int communityId, int userId)
        {
            // 1. Crear la Query con los IDs proporcionados
            var query = new CheckUserJoinedQuery(userId, communityId);

            // 2. Ejecutar la Query a través del servicio
            var isMember = await _communityQueryService.Handle(query);

            // 3. Devolver la respuesta.
            return Ok(new { isMember = isMember });
        }
        
        /// <summary>
        /// Verifica si un cliente de usuario específico es el dueño (creador) de una comunidad.
        /// </summary>
        /// <param name="communityId">El ID de la comunidad a verificar.</param>
        /// <param name="userId">El ID del cliente de usuario cuya propiedad se verifica.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un objeto JSON con el campo 'isOwner' (bool) (código 200 OK).
        /// </returns>
        [HttpGet("{communityId}/owner/{userId}/is-owner")]
        [SwaggerOperation(
            Summary= "Verificar la propiedad de un usuario en una comunidad.",
            Description= "Comprueba si el cliente de usuario especificado es el dueño (creador) de la comunidad."
        )]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> CheckUserOwner(int communityId, int userId)
        {
            // 1. Crear la Query con los IDs proporcionados
            var query = new CheckUserOwnerQuery(userId, communityId);

            // 2. Ejecutar la Query a través del servicio.
            // La lógica del servicio verifica si la comunidad existe y si OwnerId == UserId.
            var isOwner = await _communityQueryService.Handle(query);

            // 3. Devolver la respuesta encapsulada.
            return Ok(new { isOwner = isOwner });
        }
        
        /// <summary>
        /// Actualiza los detalles de una comunidad existente.
        /// Sólo el dueño de la comunidad puede realizar esta operación.
        /// </summary>
        /// <param name="id">El identificador único de la comunidad a actualizar.</param>
        /// <param name="resource">El recurso que contiene los datos actualizados de la comunidad, incluyendo el ID del dueño.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="CommunityResource"/> actualizado (código 200 OK).
        /// Retorna NotFound (404) si la comunidad no existe.
        /// Retorna BadRequest (400) si los datos son inválidos o si el usuario no es el dueño.
        /// </returns>
        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary= "Actualizar una comunidad existente.",
        Description= "Modifica los detalles de una comunidad existente. Requiere ser el dueño."
        )]
        [ProducesResponseType(typeof(CommunityResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CommunityResource>> UpdateCommunity(int id, [FromBody] UpdateCommunityResource resource)
        {
            // Creamos el comando incluyendo el ID de la ruta y los datos del recurso.
            // Asumo que UpdateCommunityResource incluye el UserClientId.
            var command = new UpdateCommunityCommand(
                CommunityId: id,
                UserClientId: resource.OwnerId,
                Name: resource.Name,
                Description: resource.Description,
                Type: resource.Type,
                Image: resource.Image,
                Banner: resource.Banner
                );
            try
            { 
                var community = await _communityCommandService.Handle(command);
                var communityResource = _mapper.Map<CommunityResource>(community);
                return Ok(communityResource);
            }
            catch (ArgumentException ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ApplicationException ex) when (ex.Message.Contains("not the owner"))
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex) 
            { 
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Elimina una comunidad existente.
        /// Sólo el dueño de la comunidad puede realizar esta operación.
        /// </summary>
        /// <param name="id">El identificador único de la comunidad a eliminar.</param>
        /// <param name="ownerId">El identificador único del cliente de usuario que intenta eliminar la comunidad (debería ser el dueño).</param>
        /// <returns>
        /// Un No Content (204) si la eliminación fue exitosa.
        /// Retorna NotFound (404) si la comunidad no existe.
        /// Retorna BadRequest (400) si el usuario no es el dueño.
        /// </returns>
        [HttpDelete("{id}/{ownerId}")] 
        [SwaggerOperation(
        Summary= "Eliminar una comunidad existente.",
        Description= "Elimina una comunidad del sistema. Requiere ser el dueño."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCommunity(int id, int ownerId)
        {
            var command = new DeleteCommunityCommand(UserClientId: ownerId, CommunityId: id);
            try
            {
                bool result = await _communityCommandService.Handle(command);
                if (!result)
                {
                    // El servicio devuelve false si la comunidad no se encontró
                    return NotFound(new { message = $"Community with ID {id} not found." });
                }
                return NoContent();
            }
            catch (ApplicationException ex) when (ex.Message.Contains("not the owner"))
            {
                // Maneja el caso de que el usuario no sea el dueño (403 Forbidden, aunque 400 también es común)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Obtiene los detalles de la comunidad, incluyendo el dueño y la lista completa de miembros.
        /// </summary>
        /// <param name="communityId">El identificador único de la comunidad.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene <see cref="CommunityMembersDetailsResource"/> (código 200 OK).
        /// Retorna 404 Not Found si la comunidad no existe.
        /// </returns>
        [HttpGet("{communityId}/members/list")]
        [SwaggerOperation(
        Summary= "Obtener detalles del dueño y lista de miembros de una comunidad.",
        Description= "Devuelve una lista enriquecida de los miembros de la comunidad, discriminando al dueño."
        )]
        [ProducesResponseType(typeof(CommunityMembersDetailsResource), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<ActionResult<CommunityMembersDetailsResource>> GetCommunityMembersDetails(int communityId)
        {
            var query = new GetCommunityMembersQuery (communityId);
            
            try
            {
                var detailsResource = await _communityQueryService.Handle(query);

                if (detailsResource == null)
                {
                    return NotFound(new { message = $"Community with ID {communityId} not found." });
                }

                return Ok(detailsResource);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}