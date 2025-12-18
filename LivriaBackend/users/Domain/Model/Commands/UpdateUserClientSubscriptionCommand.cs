namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para actualizar el plan de suscripción de un cliente de usuario.
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario cuyo plan se va a actualizar.</param>
    /// <param name="NewSubscriptionPlan">El nuevo plan de suscripción (ej. "freeplan", "communityplan").</param>
    public record UpdateUserClientSubscriptionCommand(
        int UserClientId,
        string NewSubscriptionPlan
    );
}