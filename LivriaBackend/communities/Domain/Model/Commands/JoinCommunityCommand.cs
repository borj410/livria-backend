namespace LivriaBackend.communities.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para que un cliente de usuario se una a una comunidad específica.
    /// Este comando establece una relación de membresía entre un usuario y una comunidad.
    /// </summary>
    /// <param name="UserClientId">El identificador único del cliente de usuario que desea unirse a la comunidad.</param>
    /// <param name="CommunityId">El identificador único de la comunidad a la que el usuario desea unirse.</param>
    public record JoinCommunityCommand(
        int UserClientId,
        int CommunityId
    );
}