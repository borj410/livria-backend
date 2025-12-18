using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.users.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para eliminar un libro de la lista de exclusión de un cliente de usuario.
    /// Esto se utiliza cuando el usuario decide que un libro previamente excluido puede volver a ser recomendado o visible.
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario del que se eliminará el libro.</param>
    /// <param name="BookId">El identificador único del libro que se eliminará de la lista de exclusión.</param>
    public record RemoveExclusionBookCommand(
        [Required] int UserClientId,
        [Required] int BookId
    );
}