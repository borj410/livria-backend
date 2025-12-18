using LivriaBackend.notifications.Domain.Model.Aggregates;
using LivriaBackend.notifications.Domain.Model.Queries;
using LivriaBackend.notifications.Domain.Model.Repositories;
using LivriaBackend.notifications.Domain.Model.Services; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.notifications.Application.Internal.QueryServices
{
    /// <summary>
    /// Implementa el servicio de consulta para las operaciones de la entidad <see cref="Notification"/>.
    /// Encapsula la lógica de negocio para recuperar datos de notificaciones.
    /// </summary>
    public class NotificationQueryService : INotificationQueryService
    {
        private readonly INotificationRepository _notificationRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="NotificationQueryService"/>.
        /// </summary>
        /// <param name="notificationRepository">El repositorio para las operaciones de datos de la notificación.</param>
        public NotificationQueryService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        /// <summary>
        /// Maneja la consulta para obtener una notificación por su identificador único.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID de la notificación.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Notification"/> encontrada,
        /// o <c>null</c> si no se encuentra ninguna notificación con el ID especificado.
        /// </returns>
        public async Task<Notification> Handle(GetNotificationByIdQuery query)
        {
            return await _notificationRepository.GetByIdAsync(query.NotificationId);
        }

        /// <summary>
        /// Maneja la consulta para obtener todas las notificaciones activas para un cliente de usuario específico.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID del cliente de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="Notification"/>
        /// que están activas para el <see cref="LivriaBackend.users.Domain.Model.Aggregates.UserClient"/> especificado.
        /// Retorna una colección vacía si no hay notificaciones activas para el usuario.
        /// </returns>
        public async Task<IEnumerable<Notification>> Handle(GetAllNotificationsByUserIdQuery query)
        {
            return await _notificationRepository.GetActiveByUserIdAsync(query.UserClientId);
        }
    }
}