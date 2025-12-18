namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para actualizar los datos de un cliente de usuario existente.
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario a actualizar.</param>
    /// <param name="Display">El nuevo nombre visible o alias del cliente.</param>
    /// <param name="Username">El nuevo nombre de usuario del cliente.</param>
    /// <param name="Email">La nueva dirección de correo electrónico del cliente.</param>
    /// <param name="Icon">El nuevo URL o identificador del icono/avatar del cliente.</param>
    /// <param name="Phrase">La nueva frase o estado personal del cliente.</param>
    public record UpdateUserClientCommand(
        int UserClientId,
        string Display,
        string Email,
        string Icon,
        string Phrase);
}