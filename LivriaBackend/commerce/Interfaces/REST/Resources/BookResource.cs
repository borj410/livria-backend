using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LivriaBackend.commerce.Interfaces.REST.Resources; 

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    public record BookResource(
        [Range(0, int.MaxValue, ErrorMessage = "MinimumValueError")]
        int Id,
        
        [StringLength(255, ErrorMessage = "MaxLengthError")]
        string Title,
        
        [StringLength(500, ErrorMessage = "MaxLengthError")]
        string Description,
        
        [StringLength(255, ErrorMessage = "MaxLengthError")]
        string Author,
        
        [Range(0.01, 10000.00, ErrorMessage = "RangeError")]
        decimal SalePrice,
        
        [Range(0.01, 10000.00, ErrorMessage = "RangeError")]
        decimal PurchasePrice,
        
        [Range(1, int.MaxValue, ErrorMessage = "MinimumValueError")]
        int Stock,
        
        string Cover,
        
        [StringLength(50, ErrorMessage = "MaxLengthError")]
        string Genre,
        
        [StringLength(50, ErrorMessage = "MaxLengthError")]
        string Language
        
    );
}