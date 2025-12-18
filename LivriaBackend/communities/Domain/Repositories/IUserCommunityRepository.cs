using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.shared.Domain.Repositories; 
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Repositories
{
    /// <summary>
    /// Define las operaciones de repositorio para la entidad agregada <see cref="UserCommunity"/>.
    /// Hereda las operaciones CRUD asíncronas básicas de <see cref="IAsyncRepository{T}"/>
    /// y añade métodos específicos para gestionar las membresías de usuarios en comunidades.
    /// </summary>
    public interface IUserCommunityRepository : IAsyncRepository<UserCommunity> 
    {
        /// <summary>
        /// Obtiene una relación de membresía de usuario-comunidad específica por los IDs del cliente de usuario y la comunidad.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <param name="communityId">El identificador único de la comunidad.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es el objeto <see cref="UserCommunity"/> que representa la membresía,
        /// o <c>null</c> si no se encuentra ninguna membresía con los IDs proporcionados.
        /// </returns>
        Task<UserCommunity> GetByUserAndCommunityIdsAsync(int userClientId, int communityId);
    }
}