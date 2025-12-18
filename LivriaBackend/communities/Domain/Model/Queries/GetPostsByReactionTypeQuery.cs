using LivriaBackend.communities.Domain.Model.ValueObjects;

namespace LivriaBackend.communities.Domain.Model.Queries
{
    public record GetPostsByReactionTypeQuery(int UserId, ReactionType Type);
}