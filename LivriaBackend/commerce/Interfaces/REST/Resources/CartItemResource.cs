using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    public record CartItemResource(
        [Range(0, int.MaxValue, ErrorMessage = "MinimumValueError")]
        int Id,
        
        BookResource Book, 
        
        [Range(0, 5, ErrorMessage = "RangeError")]
        int Quantity,
        
        [Range(0, int.MaxValue, ErrorMessage = "MinimumValueError")]
        int UserClientId 
    );
}