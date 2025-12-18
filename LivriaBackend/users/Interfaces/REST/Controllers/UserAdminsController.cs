using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using LivriaBackend.users.Domain.Model.Commands;
using LivriaBackend.users.Domain.Model.Queries;
using LivriaBackend.users.Domain.Model.Services;
using LivriaBackend.users.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace LivriaBackend.users.Interfaces.REST.Controllers
{
    /// <summary>
    /// Controlador RESTful para la gestión de administradores de usuario.
    /// Expone endpoints para operaciones relacionadas con <see cref="UserAdmin"/>, como
    /// obtener todos los administradores y actualizar un administrador existente.
    /// </summary>
    [Authorize(Roles = "UserClient,Admin")]
    [ApiController]
    [Route("api/v1/useradmins")]
    public class UserAdminsController : ControllerBase
    {
        private readonly IUserAdminCommandService _userAdminCommandService;
        private readonly IUserAdminQueryService _userAdminQueryService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserAdminsController"/>.
        /// </summary>
        /// <param name="userAdminCommandService">El servicio de comandos para administradores de usuario.</param>
        /// <param name="userAdminQueryService">El servicio de consultas para administradores de usuario.</param>
        /// <param name="mapper">La instancia de AutoMapper para el mapeo entre objetos.</param>
        public UserAdminsController(
            IUserAdminCommandService userAdminCommandService,
            IUserAdminQueryService userAdminQueryService,
            IMapper mapper)
        {
            _userAdminCommandService = userAdminCommandService;
            _userAdminQueryService = userAdminQueryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los administradores de usuario del sistema.
        /// </summary>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="UserAdminResource"/>
        /// si la operación fue exitosa (código 200 OK).
        /// </returns>
        [HttpGet]
        [SwaggerOperation(
            Summary= "Obtener los datos del usuario administrador.",
            Description= "Te muestra los datos del usuario administrador."
            
        )]
        [ProducesResponseType(typeof(IEnumerable<UserAdminResource>), 200)]
        public async Task<ActionResult<IEnumerable<UserAdminResource>>> GetAllUserAdmins()
        {
            var query = new GetAllUserAdminQuery();
            var userAdmins = await _userAdminQueryService.Handle(query);
            var userAdminResources = _mapper.Map<IEnumerable<UserAdminResource>>(userAdmins);
            return Ok(userAdminResources);
        }

        /// <summary>
        /// Actualiza los datos de un administrador de usuario existente.
        /// </summary>
        /// <param name="id">El identificador único del administrador de usuario a actualizar.</param>
        /// <param name="resource">Los datos actualizados del administrador de usuario en formato <see cref="UpdateUserAdminResource"/>.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="UserAdminResource"/> actualizado
        /// si la operación fue exitosa (código 200 OK).
        /// Retorna 400 Bad Request si la solicitud es inválida o el administrador no se encuentra.
        /// Retorna 500 Internal Server Error si ocurre un error inesperado.
        /// </returns>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary= "Actualizar los datos del UserAdmin.",
            Description= "Te permite modificar los datos del UserAdmin."
        )]
        [ProducesResponseType(typeof(UserAdminResource), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserAdminResource>> UpdateUserAdmin(int id, [FromBody] UpdateUserAdminResource resource)
        {
            var command = new UpdateUserAdminCommand(
                id,
                resource.Display,
                resource.Username,
                resource.Email,
                resource.AdminAccess,
                resource.SecurityPin
            );

            try
            {
                var updatedUserAdmin = await _userAdminCommandService.Handle(command);
                var userAdminResource = _mapper.Map<UserAdminResource>(updatedUserAdmin);
                return Ok(userAdminResource);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while updating the user admin.");
            }
        }
    }
}