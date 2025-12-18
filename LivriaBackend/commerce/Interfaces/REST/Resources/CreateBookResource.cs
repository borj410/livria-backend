using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Interfaces.REST.Resources
{
    public record CreateBookResource(
        [Required(ErrorMessage = "EmptyField")]
        [StringLength(255, ErrorMessage = "MaxLengthError")]
        string Title,
        
        [StringLength(1000, ErrorMessage = "MaxLengthError")]
        string Description,
        
        [Required(ErrorMessage = "EmptyField")]
        [StringLength(255, ErrorMessage = "MaxLengthError")]
        string Author,
        
        [Required(ErrorMessage = "EmptyField")]
        [Range(1, 10000, ErrorMessage = "RangeError")]
        int Stock,
        
        string Cover,
        
        [StringLength(50, ErrorMessage = "MaxLengthError")]
        string Genre,
        
        [StringLength(50, ErrorMessage = "MaxLengthError")]
        string Language
    );
}