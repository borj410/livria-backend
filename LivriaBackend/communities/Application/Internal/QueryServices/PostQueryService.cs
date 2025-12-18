using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Services;
using LivriaBackend.communities.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Application.Internal.QueryServices
{
    /// <summary>
    /// Implementa el servicio de consulta para las operaciones de la entidad <see cref="Post"/>.
    /// Encapsula la lógica de negocio para recuperar datos de publicaciones.
    /// </summary>
    public class PostQueryService : IPostQueryService
    {
        private readonly IPostRepository _postRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostQueryService"/>.
        /// </summary>
        /// <param name="postRepository">El repositorio para las operaciones de datos de publicaciones.</param>
        public PostQueryService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        /// <summary>
        /// Maneja el comando para obtener todas las publicaciones.
        /// </summary>
        /// <param name="query">La consulta para obtener todas las publicaciones.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de todas las <see cref="Post"/>.</returns>
        public async Task<IEnumerable<Post>> Handle(GetAllPostsQuery query)
        {
            return await _postRepository.ListAsync(); 
        }

        /// <summary>
        /// Maneja el comando para obtener una publicación por su identificador único.
        /// </summary>
        /// <param name="query">La consulta para obtener una publicación por ID.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Post"/> encontrada, o null si no existe.</returns>
        public async Task<Post> Handle(GetPostByIdQuery query)
        {
            return await _postRepository.GetByIdAsync(query.PostId); 
        }

        /// <summary>
        /// Maneja el comando para obtener todas las publicaciones de una comunidad específica.
        /// </summary>
        /// <param name="query">La consulta para obtener publicaciones por ID de comunidad.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="Post"/> para la comunidad especificada.</returns>
        public async Task<IEnumerable<Post>> Handle(GetPostsByCommunityIdQuery query)
        {
            return await _postRepository.GetByCommunityIdAsync(query.CommunityId);
        }
        
        public async Task<bool> Handle(CheckPostAuthorQuery query)
        {
            var post = await _postRepository.GetByIdAsync(query.PostId);
            
            if (post == null)
            {
                return false;
            }
            
            return post.UserId == query.UserId;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId)
        {
            return await _postRepository.GetPostsByUserIdAsync(userId);
        }
    }
}