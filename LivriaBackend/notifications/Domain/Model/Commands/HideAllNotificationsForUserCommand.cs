namespace LivriaBackend.notifications.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para ocultar todas las notificaciones de un cliente de usuario específico.
    /// </summary>
    /// <param name="UserClientId">El identificador del cliente de usuario cuyas notificaciones se desean ocultar.</param>
    public record HideAllNotificationsForUserCommand(int UserClientId);
}