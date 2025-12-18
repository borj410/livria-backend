namespace LivriaBackend.communities.Domain.Model.Queries
{
    /// <summary>
    /// Consulta utilizada para verificar si un usuario de tipo cliente específico es miembro de una comunidad específica.
    /// </summary>
    /// <param name="UserClientId">El ID del usuario cliente a verificar.</param>
    /// <param name="CommunityId">El ID de la comunidad a verificar.</param>
    public record CheckUserJoinedQuery(int UserClientId, int CommunityId);
}