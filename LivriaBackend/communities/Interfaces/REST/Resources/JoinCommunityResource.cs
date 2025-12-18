using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.communities.Interfaces.REST.Resources
{

    public record JoinCommunityResource(
        [Required(ErrorMessage = "EmptyField")]
        int UserClientId,
        [Required(ErrorMessage = "EmptyField")]
        int CommunityId
    );
}