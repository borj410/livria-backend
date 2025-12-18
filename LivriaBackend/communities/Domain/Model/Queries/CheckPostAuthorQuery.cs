namespace LivriaBackend.communities.Domain.Model.Queries
{
    /// <summary>
    /// Representa una consulta para verificar si un cliente de usuario es el autor de una publicación específica.
    /// </summary>
    /// <param name="PostId">El identificador único de la publicación.</param>
    /// <param name="UserId">El identificador del usuario de tipo cliente a verificar.</param>
    public record CheckPostAuthorQuery(
        int PostId,
        int UserId
    );
}