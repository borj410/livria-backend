using LivriaBackend.communities.Domain.Model.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.communities.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para crear, actualizar o eliminar una reacción a un post.
    /// </summary>
    public record PostReactionCommand(
        [Required] int UserId,
        [Required] int PostId,
        [Required] ReactionType Type
    );
}