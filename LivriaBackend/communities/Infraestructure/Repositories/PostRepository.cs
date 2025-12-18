// Path: LivriaBackend.communities.Infrastructure.Repositories/PostRepository.cs
using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Aggregates; // Added for UserClient navigation property

namespace LivriaBackend.communities.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa el repositorio para la entidad <see cref="Post"/>.
    /// Proporciona métodos para la persistencia y recuperación de datos de publicaciones,
    /// incluyendo la carga de relaciones y filtrado por comunidad.
    /// </summary>
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación.</param>
        public PostRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene una colección de todas las publicaciones que pertenecen a una comunidad específica.
        /// Incluye los datos del usuario cliente (<see cref="Post.UserClient"/>) y la comunidad (<see cref="Post.Community"/>) asociados.
        /// </summary>
        /// <param name="communityId">El identificador único de la comunidad.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es una colección de <see cref="Post"/> asociadas a la comunidad especificada,
        /// con sus relaciones de usuario y comunidad cargadas de forma ansiosa.
        /// Retorna una colección vacía si no se encuentran publicaciones para el ID de comunidad dado.
        /// </returns>
        public async Task<IEnumerable<Post>> GetByCommunityIdAsync(int communityId)
        {
            return await this.Context.Set<Post>()
                .Include(p => p.UserClient)
                .Include(p => p.Community)
                .Where(p => p.CommunityId == communityId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
        {
            return await Context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
    }
}