using LivriaBackend.communities.Domain.Model.ValueObjects;

namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    public record UserReactionStatusResource(
        ReactionType Status // None, Like, or Dislike
    );
}