using LivriaBackend.communities.Domain.Model.ValueObjects; 

namespace LivriaBackend.communities.Domain.Model.Commands
{
    public record UpdateCommunityCommand(
        int CommunityId,
        int UserClientId,
        string Name,
        string Description,
        CommunityType Type, 
        string Image, 
        string Banner
    );
}