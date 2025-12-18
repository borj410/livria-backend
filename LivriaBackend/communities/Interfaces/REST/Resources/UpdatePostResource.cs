using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    /// <summary>
    /// Recurso para actualizar el contenido de una publicación existente.
    /// </summary>
    public record UpdatePostResource(
        [Required] int UserId,
        [Required] string Content,
        string? Img // Se permite nulo si no se cambia la imagen
    );
}