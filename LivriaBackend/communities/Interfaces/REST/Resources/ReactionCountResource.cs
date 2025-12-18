namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    public record ReactionCountsResource(
        int Likes,
        int Dislikes
    );
}