using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para actualizar el stock de un libro existente añadiendo una cantidad específica.
    /// </summary>
    /// <param name="BookId">El identificador único del libro cuyo stock se actualizará.</param>
    /// <param name="QuantityToAdd">La cantidad a añadir al stock actual del libro. Debe ser un valor positivo.</param>
    public record UpdateBookStockCommand(
        [Required] int BookId,
        [Required] int QuantityToAdd 
    );
}