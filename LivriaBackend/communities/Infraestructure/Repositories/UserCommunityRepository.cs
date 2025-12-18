using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Repositories; 
using Microsoft.EntityFrameworkCore; 
using System.Threading.Tasks;

namespace LivriaBackend.communities.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa el repositorio para la entidad agregada <see cref="UserCommunity"/>.
    /// Proporciona métodos para la persistencia y recuperación de datos de las relaciones
    /// entre usuarios y comunidades.
    /// </summary>
    public class UserCommunityRepository : BaseRepository<UserCommunity>, IUserCommunityRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserCommunityRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación.</param>
        public UserCommunityRepository(AppDbContext context) : base(context)
        {
        }

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
        public async Task<UserCommunity> GetByUserAndCommunityIdsAsync(int userClientId, int communityId)
        {
            return await Context.UserCommunities 
                .FirstOrDefaultAsync(uc => uc.UserClientId == userClientId && uc.CommunityId == communityId);
        }
        
    }
}