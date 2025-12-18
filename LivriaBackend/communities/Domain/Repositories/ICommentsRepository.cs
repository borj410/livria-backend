using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.shared.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Repositories
{
    public interface ICommentRepository : IAsyncRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
        Task<(int PostId, int AuthorUserId)?> GetPostAndAuthorIdsByIdAsync(int commentId);
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
    }
}