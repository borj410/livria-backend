using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para eliminar un ítem específico del carrito de compras de un usuario.
    /// </summary>
    /// <param name="CartItemId">El identificador único del ítem del carrito a eliminar.</param>
    /// <param name="UserClientId">El identificador único del cliente de usuario propietario del carrito.</param>
    public record RemoveCartItemCommand(
        [Required] int CartItemId,
        [Required] int UserClientId
    );
}