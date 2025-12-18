using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    /// <summary>
    /// Recurso DTO para actualizar una reseña existente.
    /// Contiene los campos modificables por el autor de la reseña.
    /// </summary>
    public record UpdateReviewResource
    {
        /// <summary>
        /// El nuevo contenido textual de la reseña.
        /// </summary>
        [Required]
        [StringLength(5000, MinimumLength = 10)]
        public string Content { get; init; }

        /// <summary>
        /// La nueva calificación por estrellas de la reseña (de 1 a 5).
        /// </summary>
        [Required]
        [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5.")]
        public int Stars { get; init; }
    }
}