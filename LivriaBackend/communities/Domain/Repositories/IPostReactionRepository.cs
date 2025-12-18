using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.ValueObjects;
using LivriaBackend.shared.Domain.Repositories;

namespace LivriaBackend.communities.Domain.Repositories
{
    public interface IPostReactionRepository : IAsyncRepository<PostReaction>
    {
        // Obtiene una reacción específica de un usuario a un post.
        Task<PostReaction> GetByUserIdAndPostIdAsync(int userId, int postId);
        
        Task<(int Likes, int Dislikes)> GetReactionCountsByPostIdAsync(int postId);
        
        Task<IEnumerable<int>> GetPostIdsByUserIdAndReactionTypeAsync(int userId, ReactionType type);
    }   
}