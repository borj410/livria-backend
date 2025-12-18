using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.users.Interfaces.REST.Resources
{
    public record UpdateUserClientSubscriptionResource(
        [Required(ErrorMessage = "EmptyField")]
        [StringLength(50, ErrorMessage = "MaxLengthError")] 
        string NewSubscriptionPlan
    );
}