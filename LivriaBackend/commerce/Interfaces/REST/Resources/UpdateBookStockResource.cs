using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    /// <summary>
    /// Resource para añadir stock a un libro.
    /// </summary>
    public class UpdateBookStockResource
    {
        /// <summary>
        /// La cantidad a añadir al stock actual del libro. Debe ser un valor positivo.
        /// </summary>
        [Required(ErrorMessage = "La cantidad a añadir es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad a añadir debe ser mayor a cero.")] 
        public int QuantityToAdd { get; set; } 
    }
}