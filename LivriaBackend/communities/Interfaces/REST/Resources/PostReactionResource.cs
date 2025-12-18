using LivriaBackend.communities.Domain.Model.ValueObjects;
using System;

namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    /// <summary>
    /// Recurso de respuesta para representar una reacción de usuario a un post.
    /// </summary>
    public record PostReactionResource(
        int Id,
        int UserId,
        int PostId,
        ReactionType Type,
        DateTime UpdatedAt
    );
}