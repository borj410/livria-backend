namespace LivriaBackend.communities.Domain.Model.Commands
{
    public record LeaveCommunityCommand(int UserClientId, int CommunityId);
}