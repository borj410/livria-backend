using LivriaBackend.communities.Domain.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    /// <summary>
    /// Recurso para registrar, actualizar o eliminar una reacción a un post.
    /// </summary>
    public record ReactToPostResource(
        [Required] int UserId,
        [Required] ReactionType Type // 1: Like, 2: Dislike, 0: None
    );
}