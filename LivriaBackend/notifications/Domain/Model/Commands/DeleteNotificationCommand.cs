namespace LivriaBackend.notifications.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para eliminar una notificación específica del sistema.
    /// </summary>
    /// <param name="NotificationId">El identificador único de la notificación a eliminar.</param>
    public record DeleteNotificationCommand(
        int NotificationId
    );
}