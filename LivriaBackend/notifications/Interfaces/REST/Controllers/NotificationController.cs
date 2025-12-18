using AutoMapper;
using LivriaBackend.notifications.Domain.Model.Aggregates;
using LivriaBackend.notifications.Domain.Model.Commands;
using LivriaBackend.notifications.Domain.Model.Queries;
using LivriaBackend.notifications.Domain.Model.Services;
using LivriaBackend.notifications.Interfaces.REST.Resources; 
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace LivriaBackend.notifications.Interfaces.REST.Controllers
{
    /// <summary>
    /// Controlador RESTful para gestionar las operaciones relacionadas con las notificaciones.
    /// </summary>
    [Authorize(Roles = "UserClient,Admin")]
    [ApiController]
    [Route("api/v1/notifications")]
    [Produces(MediaTypeNames.Application.Json)]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationCommandService _notificationCommandService;
        private readonly INotificationQueryService _notificationQueryService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="NotificationController"/>.
        /// </summary>
        /// <param name="notificationCommandService">El servicio de comandos de notificaciones.</param>
        /// <param name="notificationQueryService">El servicio de consulta de notificaciones.</param>
        /// <param name="mapper">La instancia de AutoMapper para la transformación de objetos.</param>
        public NotificationController(
            INotificationCommandService notificationCommandService,
            INotificationQueryService notificationQueryService,
            IMapper mapper)
        {
            _notificationCommandService = notificationCommandService;
            _notificationQueryService = notificationQueryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Crea una nueva notificación en el sistema.
        /// </summary>
        /// <param name="resource">El recurso que contiene los datos de la notificación a crear.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="NotificationResource"/> de la notificación creada
        /// con un código 201 CreatedAtAction si la operación es exitosa.
        /// Retorna BadRequest (400) si la notificación no pudo ser creada debido a datos inválidos.
        /// </returns>
        [HttpPost]
        [SwaggerOperation(
            Summary= "Crear una nueva notificación.",
            Description= "Crea una nueva notificación en el sistema."
        )]
        [ProducesResponseType(typeof(NotificationResource), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NotificationResource>> CreateNotification([FromBody] CreateNotificationResource resource)
        {
            var createCommand = _mapper.Map<CreateNotificationCommand>(resource);
            
            
            if (createCommand.CreatedAt == default(DateTime)) {
                createCommand = createCommand with { CreatedAt = DateTime.UtcNow };
            }

            Notification notification = await _notificationCommandService.Handle(createCommand);
            var notificationResource = _mapper.Map<NotificationResource>(notification);
            return CreatedAtAction(nameof(GetNotificationById), new { id = notification.Id }, notificationResource);
        }

        /// <summary>
        /// Obtiene todas las notificaciones activas (no ocultas) de un usuario específico.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="NotificationResource"/>
        /// si la operación es exitosa (código 200 OK). Puede ser una colección vacía si no hay notificaciones activas para el usuario.
        /// </returns>
        [HttpGet("user/{userClientId}")] 
        [SwaggerOperation(
            Summary= "Obtener las notificaciones activas de un usuario.",
            Description= "Muestra las notificaciones que no han sido ocultadas para un usuario específico."
        )]
        [ProducesResponseType(typeof(IEnumerable<NotificationResource>), 200)]
        public async Task<ActionResult<IEnumerable<NotificationResource>>> GetActiveNotificationsByUserId(int userClientId)
        {
            var query = new GetAllNotificationsByUserIdQuery(userClientId); 
            var notifications = await _notificationQueryService.Handle(query);
            var resources = _mapper.Map<IEnumerable<NotificationResource>>(notifications);
            return Ok(resources);
        }

        /// <summary>
        /// Obtiene los datos de una notificación específica por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la notificación.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un <see cref="NotificationResource"/> si la notificación es encontrada (código 200 OK),
        /// o un resultado NotFound (código 404) si la notificación no existe.
        /// </returns>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary= "Obtener los datos de una notificación en específico.",
            Description= "Te muestra los datos de la notificación que buscaste."
        )]
        [ProducesResponseType(typeof(NotificationResource), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NotificationResource>> GetNotificationById(int id)
        {
            var query = new GetNotificationByIdQuery(id);
            var notification = await _notificationQueryService.Handle(query);

            if (notification == null)
            {
                return NotFound();
            }

            var resource = _mapper.Map<NotificationResource>(notification);
            return Ok(resource);
        }

        /// <summary>
        /// Oculta todas las notificaciones activas de un usuario.
        /// Esto marca las notificaciones como "ocultas" (eliminación lógica) y no las elimina físicamente del sistema.
        /// </summary>
        /// <param name="resource">El recurso que contiene el ID del cliente de usuario cuyas notificaciones se desean ocultar.</param>
        /// <returns>
        /// Un resultado NoContent (código 204) si la operación fue exitosa.
        /// Retorna BadRequest (400) si hay un error en el procesamiento (ej. validación, error de negocio).
        /// Retorna Unauthorized (401) si la autenticación falla (aunque esto suele ser manejado por middleware).
        /// </returns>
        [HttpPatch("hide-all")] 
        [SwaggerOperation(
            Summary= "Ocultar todas las notificaciones de un usuario.",
            Description= "Marca todas las notificaciones activas de un usuario como ocultas (eliminación lógica). Las notificaciones no serán eliminadas físicamente del sistema."
        )]
        [ProducesResponseType(204)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(401)] 
        public async Task<IActionResult> HideAllNotificationsForUser([FromBody] HideAllNotificationsForUserResource resource)
        {
            var command = new HideAllNotificationsForUserCommand(resource.UserClientId);

            try
            {
                await _notificationCommandService.Handle(command);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}