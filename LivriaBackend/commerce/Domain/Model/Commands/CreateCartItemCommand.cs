using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para añadir un nuevo ítem al carrito de compras de un usuario.
    /// </summary>
    /// <param name="BookId">El identificador único del libro a añadir al carrito.</param>
    /// <param name="Quantity">La cantidad del libro a añadir. Debe ser al menos 1.</param>
    /// <param name="UserClientId">El identificador único del cliente de usuario al que pertenece el carrito.</param>
    public record CreateCartItemCommand(
        [Required] int BookId,
        [Required] [Range(1, int.MaxValue)] int Quantity,
        [Required] int UserClientId
    );
}