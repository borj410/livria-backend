namespace LivriaBackend.communities.Domain.Model.Queries
{
    /// <summary>
    /// Consulta para obtener todos los comentarios asociados a un Post específico.
    /// </summary>
    public record GetCommentsByPostIdQuery(int PostId);
}