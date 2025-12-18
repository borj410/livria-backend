using LivriaBackend.notifications.Domain.Model.Aggregates;
using LivriaBackend.notifications.Domain.Model.Commands;
using LivriaBackend.notifications.Domain.Model.Repositories;
using LivriaBackend.notifications.Domain.Model.Services;
using LivriaBackend.shared.Domain.Repositories; 
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LivriaBackend.notifications.Application.Internal.CommandServices
{
    /// <summary>
    /// Implementa el servicio de comandos para las operaciones de la entidad <see cref="Notification"/>.
    /// Encapsula la lógica de negocio para crear y gestionar notificaciones.
    /// </summary>
    public class NotificationCommandService : INotificationCommandService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="NotificationCommandService"/>.
        /// </summary>
        /// <param name="notificationRepository">El repositorio para las operaciones de datos de la notificación.</param>
        /// <param name="unitOfWork">La unidad de trabajo para gestionar las transacciones de base de datos.</param>
        public NotificationCommandService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Maneja el comando para crear una nueva notificación.
        /// </summary>
        /// <param name="command">El comando que contiene los datos para crear la notificación.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Notification"/> recién creada.</returns>
        public async Task<Notification> Handle(CreateNotificationCommand command)
        {
            
            var notification = new Notification(command.UserClientId, command.Type, command.CreatedAt); 
            
            
            await _notificationRepository.AddAsync(notification);
            
            
            await _unitOfWork.CompleteAsync();
            
            return notification;
        }

        /// <summary>
        /// Maneja el comando para ocultar todas las notificaciones activas de un usuario específico.
        /// Esto marca las notificaciones como "ocultas" o "leídas" en el sistema.
        /// </summary>
        /// <param name="command">El comando que contiene el ID del cliente de usuario para quien se ocultarán las notificaciones.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task Handle(HideAllNotificationsForUserCommand command)
        {
            
            IEnumerable<Notification> activeNotifications = await _notificationRepository.GetActiveByUserIdAsync(command.UserClientId);
            
            
            if (activeNotifications == null)
            {
                return; 
            }
            
            foreach (var notification in activeNotifications)
            {
                notification.Hide(); 
                await _notificationRepository.UpdateAsync(notification); 
            }
            
            await _unitOfWork.CompleteAsync(); 
        }
    }
}