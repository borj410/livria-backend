using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para actualizar la cantidad de un ítem existente en el carrito de compras de un usuario.
    /// </summary>
    /// <param name="CartItemId">El identificador único del ítem del carrito a actualizar.</param>
    /// <param name="NewQuantity">La nueva cantidad deseada para el ítem. Si es 0, el ítem se eliminará.</param>
    /// <param name="UserClientId">El identificador único del cliente de usuario propietario del carrito.</param>
    public record UpdateCartItemQuantityCommand(
        [Required] int CartItemId,
        [Required] [Range(0, int.MaxValue)] int NewQuantity,
        [Required] int UserClientId
    );
}