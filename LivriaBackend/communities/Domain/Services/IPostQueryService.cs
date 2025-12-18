using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Model.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Services
{
    public interface IPostQueryService
    {
        Task<IEnumerable<Post>> Handle(GetAllPostsQuery query);
        Task<Post> Handle(GetPostByIdQuery query);
        Task<IEnumerable<Post>> Handle(GetPostsByCommunityIdQuery query);
        Task<bool> Handle(CheckPostAuthorQuery query);
        
        /// <summary>
        /// Consulta todas las publicaciones creadas por un usuario cliente específico.
        /// </summary>
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
    }
}