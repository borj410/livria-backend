using System;
using LivriaBackend.notifications.Domain.Model.ValueObjects;

namespace LivriaBackend.notifications.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para crear una nueva notificación.
    /// Este comando encapsula los datos esenciales para generar una notificación en el sistema.
    /// </summary>
    /// <param name="UserClientId">El identificador del cliente de usuario al que se dirigirá la notificación.</param>
    /// <param name="Type">El tipo de notificación, que influye en el título y el contenido. Ver <see cref="ENotificationType"/>.</param>
    /// <param name="CreatedAt">La fecha y hora UTC en que se creó la notificación.</param>
    public record CreateNotificationCommand(
        int UserClientId, 
        ENotificationType Type, 
        DateTime CreatedAt 
    );
}