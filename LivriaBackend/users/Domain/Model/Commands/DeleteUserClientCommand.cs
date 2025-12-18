namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para eliminar un cliente de usuario específico del sistema.
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario a eliminar.</param>
    public record DeleteUserClientCommand(
        int UserClientId
    );
}