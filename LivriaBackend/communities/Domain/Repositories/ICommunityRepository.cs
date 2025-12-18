using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.shared.Domain.Repositories; 

namespace LivriaBackend.communities.Domain.Repositories
{
    /// <summary>
    /// Define las operaciones de repositorio para el agregado <see cref="Community"/>.
    /// Hereda las operaciones CRUD asíncronas básicas de <see cref="IAsyncRepository{T}"/>.
    /// </summary>
    public interface ICommunityRepository : IAsyncRepository<Community>
    {
        /// <summary>
        /// Obtiene una comunidad por su identificador único, incluyendo la colección de miembros (UserCommunities).
        /// </summary>
        /// <param name="id">El identificador único de la comunidad.</param>
        /// <returns>La comunidad con sus miembros cargados, o null si no existe.</returns>
        Task<Community> GetByIdWithMembersAsync(int id);
        Task<IEnumerable<Community>> GetCommunitiesByOwnerIdAsync(int ownerId);
    }
}