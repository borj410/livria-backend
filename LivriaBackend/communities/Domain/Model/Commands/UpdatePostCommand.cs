namespace LivriaBackend.communities.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para actualizar el contenido de una publicación.
    /// </summary>
    /// <param name="PostId">El identificador único de la publicación a editar.</param>
    /// <param name="UserId">El identificador del usuario que intenta la edición (debe ser el autor).</param>
    /// <param name="Content">El nuevo contenido de texto de la publicación.</param>
    /// <param name="Img">La nueva URL de la imagen de la publicación.</param>
    public record UpdatePostCommand(
        int PostId,
        int UserId,
        string Content,
        string Img
    );
}