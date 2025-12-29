// LivriaBackend.IAM.Interfaces.REST.Resources/UpdateIdentitySecurityResource.cs

using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.IAM.Interfaces.REST.Resources
{
    public record UpdateIdentitySecurityResource(
        [Required]
        string CurrentPassword,
        
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        string? NewUsername, 

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 100 characters.")]
        string? NewPassword
    );
}