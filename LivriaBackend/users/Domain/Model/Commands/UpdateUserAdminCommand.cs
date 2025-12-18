namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para actualizar los datos de un administrador de usuario existente.
    /// </summary>
    /// <param name="UserAdminId">El identificador único del administrador de usuario a actualizar.</param>
    /// <param name="Display">El nuevo nombre visible o alias del administrador.</param>
    /// <param name="Username">El nuevo nombre de usuario del administrador.</param>
    /// <param name="Email">La nueva dirección de correo electrónico del administrador.</param>
    /// <param name="AdminAccess">El nuevo estado de acceso de administrador.</param>
    /// <param name="SecurityPin">El nuevo pin de seguridad del administrador.</param>
    public record UpdateUserAdminCommand(
        int UserAdminId,
        string Display,
        string Username,
        string Email,
        bool AdminAccess,
        string SecurityPin
    );
}