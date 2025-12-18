namespace LivriaBackend.communities.Domain.Model.Commands
{
    /// <summary>
    /// Comando para eliminar un comentario.
    /// Requiere la validación del autor o el dueño de la comunidad.
    /// </summary>
    public record DeleteCommentCommand(
        int CommentId,
        int UserId
    );
}