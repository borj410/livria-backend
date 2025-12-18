using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    /// <summary>
    /// Recurso para eliminar una publicación existente.
    /// </summary>
    public record DeletePostResource(
        [Required] int UserId,
        [Required] int CommunityId
    );
}