namespace LivriaBackend.communities.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para eliminar una publicación.
    /// </summary>
    /// <param name="PostId">El identificador único de la publicación a eliminar.</param>
    /// <param name="UserId">El identificador del usuario que intenta la eliminación (debe ser el autor o el dueño de la comunidad).</param>
    /// <param name="CommunityId">El identificador de la comunidad (necesario para la validación de propiedad).</param>
    public record DeletePostCommand(
        int PostId,
        int UserId,
        int CommunityId
    );
}