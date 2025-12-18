using LivriaBackend.notifications.Domain.Model.Aggregates;
using LivriaBackend.notifications.Domain.Model.Commands;
using System.Threading.Tasks;

namespace LivriaBackend.notifications.Domain.Model.Services
{
    /// <summary>
    /// Define el contrato para el servicio de comandos de notificaciones.
    /// Este servicio es responsable de orquestar las operaciones que modifican el estado de las notificaciones.
    /// </summary>
    public interface INotificationCommandService
    {
        /// <summary>
        /// Maneja el comando para crear una nueva notificación.
        /// </summary>
        /// <param name="command">El comando <see cref="CreateNotificationCommand"/> que contiene los datos para la nueva notificación.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es la <see cref="Notification"/> recién creada.
        /// </returns>
        Task<Notification> Handle(CreateNotificationCommand command);

        /// <summary>
        /// Maneja el comando para ocultar todas las notificaciones activas de un usuario específico.
        /// </summary>
        /// <param name="command">El comando <see cref="HideAllNotificationsForUserCommand"/> que identifica al usuario cuyas notificaciones se van a ocultar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task Handle(HideAllNotificationsForUserCommand command); 
    }
}