using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Services
{
    public interface ICommentQueryService
    {
        Task<IEnumerable<Comment>> Handle(GetCommentsByPostIdQuery query);
        
        /// <summary>
        /// Consulta todos los comentarios creados por un usuario cliente específico.
        /// </summary>
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
    }
}