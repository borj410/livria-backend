using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    /// <summary>
    /// Resource para actualizar el estado de una orden.
    /// </summary>
    public class UpdateOrderStatusResource
    {
        /// <summary>
        /// El nuevo estado para la orden. Debe ser 'pending', 'in progress' o 'delivered'.
        /// </summary>
        [Required(ErrorMessage = "El estado es requerido.")]
        [RegularExpression("^(pending|delivered|in progress)$", ErrorMessage = "El estado de la orden debe ser 'pending' o 'delivered'.")]
        public string Status { get; set; }
    }
}