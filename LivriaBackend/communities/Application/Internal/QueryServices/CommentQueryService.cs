using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Services;
using LivriaBackend.communities.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Application.Internal.QueryServices
{
    public class CommentQueryService : ICommentQueryService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentQueryService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        /// <summary>
        /// Maneja la consulta para obtener todos los comentarios de un PostId dado.
        /// </summary>
        /// <param name="query">La consulta que contiene el PostId.</param>
        /// <returns>Una colección de entidades Comment.</returns>
        public async Task<IEnumerable<Comment>> Handle(GetCommentsByPostIdQuery query)
        {
            return await _commentRepository.GetCommentsByPostIdAsync(query.PostId);
        }
        
        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId)
        {
            return await _commentRepository.GetCommentsByUserIdAsync(userId);
        }
    }
}