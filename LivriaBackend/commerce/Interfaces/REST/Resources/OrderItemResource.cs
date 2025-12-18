using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    public record OrderItemResource(
        [Range(0, int.MaxValue, ErrorMessage = "MinimumValueError")]
        int Id,
        
        [Range(0, int.MaxValue, ErrorMessage = "MinimumValueError")]
        int BookId, 
        
        string BookTitle,
        
        [StringLength(255, ErrorMessage = "MaxLengthError")]
        string BookAuthor,
        
        [Range(0.01, 10000.00, ErrorMessage = "RangeError")]
        decimal BookPrice,
        
        string BookCover,
        
        [Range(0, 3, ErrorMessage = "RangeError")]
        int Quantity,
        
        [Range(0.01, 10000.00, ErrorMessage = "RangeError")]
        decimal ItemTotal
    );
}