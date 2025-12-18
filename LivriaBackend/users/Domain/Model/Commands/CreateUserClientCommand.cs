namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para crear un nuevo cliente de usuario.
    /// Encapsula los datos necesarios para el registro inicial de un usuario.
    /// </summary>
    /// <param name="Display">El nombre visible o alias del nuevo usuario cliente.</param>
    /// <param name="Username">El nombre de usuario único para el inicio de sesión.</param>
    /// <param name="Email">La dirección de correo electrónico del usuario.</param>
    /// <param name="Icon">La URL o identificador del icono/avatar del usuario.</param>
    /// <param name="Phrase">Una frase o estado personal del usuario.</param>
    public record CreateUserClientCommand(
        string Display,
        string Username,
        string Email,
        string Icon, 
        string Phrase 
    );
}