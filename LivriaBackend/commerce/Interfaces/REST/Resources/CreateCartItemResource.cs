using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    public record CreateCartItemResource(
        [Required(ErrorMessage = "EmptyField")]
        int BookId,
        
        [Required(ErrorMessage = "EmptyField")]
        [Range(1, 10, ErrorMessage = "Quantity must be between 1 and 10.")] 
        int Quantity,
        
        [Required(ErrorMessage = "EmptyField")]
        [Range(0, int.MaxValue, ErrorMessage = "MinimumValueError")]
        int UserClientId
    );
}