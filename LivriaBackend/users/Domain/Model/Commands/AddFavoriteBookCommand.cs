using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para añadir un libro a la lista de favoritos de un cliente de usuario.
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario al que se añadirá el libro.</param>
    /// <param name="BookId">El identificador único del libro que se añadirá como favorito.</param>
    public record AddFavoriteBookCommand(
        [Required] int UserClientId,
        [Required] int BookId
    );
}