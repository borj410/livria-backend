using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para eliminar un libro de la lista de favoritos de un cliente de usuario.
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario del que se eliminará el libro.</param>
    /// <param name="BookId">El identificador único del libro que se eliminará de favoritos.</param>
    public record RemoveFavoriteBookCommand(
        [Required] int UserClientId,
        [Required] int BookId
    );
}