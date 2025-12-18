using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    public record UpdateBookResource(
        [StringLength(255, ErrorMessage = "MaxLengthError")]
        string Title, 
        
        [StringLength(500, ErrorMessage = "MaxLengthError")]
        string Description, 
        
        [StringLength(255, ErrorMessage = "MaxLengthError")]
        string Author, 
        
        [Range(0.01, 10000.00, ErrorMessage = "RangeError")]
        decimal PurchasePrice, 
        
        string Cover, 
        
        [StringLength(50, ErrorMessage = "MaxLengthError")]
        string Genre, 
        
        [StringLength(50, ErrorMessage = "MaxLengthError")]
        string Language);
}