using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para actualizar el estado de una orden existente.
    /// </summary>
    /// <param name="OrderId">El identificador único de la orden a actualizar.</param>
    /// <param name="Status">El nuevo estado deseado para la orden. Debe ser 'pending', 'in progress' o 'delivered'.</param>
    public record UpdateOrderStatusCommand(
        [Required] int OrderId,
        [Required] string Status
    );
}