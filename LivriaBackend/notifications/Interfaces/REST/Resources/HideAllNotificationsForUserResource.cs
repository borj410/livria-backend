using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.notifications.Interfaces.REST.Resources
{
    public record HideAllNotificationsForUserResource(
        [Required(ErrorMessage = "User Client ID is required.")]
        int UserClientId
    );
}