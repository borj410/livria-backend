using System.ComponentModel.DataAnnotations;
using LivriaBackend.shared.Resources;

namespace LivriaBackend.users.Interfaces.REST.Resources
{
    /// <summary>
    /// Recurso para la solicitud de registro de un nuevo cliente.
    /// Incluye validaciones de longitud y localización para mensajes de error.
    /// </summary>
    public record RegisterUserClientRequest
    {
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "EmptyField")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "UsernameError")]
        public string Username { get; init; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "EmptyField")]
        [StringLength(100, MinimumLength = 8, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "PasswordError")]
        [DataType(DataType.Password)]
        public string Password { get; init; }
        
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "EmptyField")]
        [Compare(nameof(Password), ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "PasswordMismatch")]
        public string ConfirmPassword { get; init; }
        
        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "EmptyField")]
        [StringLength(100, MinimumLength = 3, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "LengthError")]
        public string Display { get; init; }

        [Required(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "EmptyField")]
        [EmailAddress(ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "EmailError")]
        [StringLength(100, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "MaxLengthError")]
        public string Email { get; init; }

        public string Icon { get; init; }
        
        [StringLength(255, ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName = "MaxLengthError")]
        public string Phrase { get; init; }
    }
}