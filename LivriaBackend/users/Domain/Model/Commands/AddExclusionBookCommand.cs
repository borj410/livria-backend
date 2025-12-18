using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para añadir un libro a la lista de exclusión de un cliente de usuario.
    /// Este comando se utiliza para marcar un libro que el usuario no desea ver o recibir como recomendación.
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario al que se añadirá el libro.</param>
    /// <param name="BookId">El identificador único del libro que se añadirá a la lista de exclusión.</param>
    public record AddExclusionBookCommand(
        [Required] int UserClientId,
        [Required] int BookId
    );
}