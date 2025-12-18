namespace LivriaBackend.communities.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para que un cliente de usuario (dueño) elimine la comunidad
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario que es dueño de la comunidad.</param>
    /// <param name="CommunityId">El identificador único de la comunidad.</param>
    public record DeleteCommunityCommand(
        int UserClientId,
        int CommunityId
    );
}