using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LivriaBackend.communities.Domain.Model.ValueObjects;

namespace LivriaBackend.communities.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio para la entidad agregada <see cref="PostReaction"/>.
    /// </summary>
    public class PostReactionRepository : BaseRepository<PostReaction>, IPostReactionRepository
    {
        public PostReactionRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene una reacción específica por el ID del usuario y el ID del post.
        /// </summary>
        public async Task<PostReaction> GetByUserIdAndPostIdAsync(int userId, int postId)
        {
            return await Context.Set<PostReaction>()
                .FirstOrDefaultAsync(pr => pr.UserId == userId && pr.PostId == postId);
        }

        // --- Operaciones CRUD heredadas de IAsyncRepository (implementadas aquí ya que BaseRepository está vacío) ---
        
        public async Task AddAsync(PostReaction entity)
        {
            await Context.Set<PostReaction>().AddAsync(entity);
        }
        
        public async Task UpdateAsync(PostReaction entity)
        {
            Context.Set<PostReaction>().Update(entity);
            await Task.CompletedTask;
        }
        
        public async Task DeleteAsync(PostReaction entity)
        {
            Context.Set<PostReaction>().Remove(entity);
            await Task.CompletedTask;
        }
        
        public async Task<PostReaction> GetByIdAsync(int id)
        {
             return await Context.Set<PostReaction>().FirstOrDefaultAsync(pr => pr.Id == id);
        }
        
        public Task<IEnumerable<PostReaction>> ListAsync()
        {
            throw new NotImplementedException(); 
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Context.Set<PostReaction>().AnyAsync(pr => pr.Id == id);
        }
        
        public async Task<(int Likes, int Dislikes)> GetReactionCountsByPostIdAsync(int postId)
        {
            var reactions = await Context.Set<PostReaction>()
                .Where(pr => pr.PostId == postId)
                .ToListAsync();

            var likes = reactions.Count(pr => pr.Type == ReactionType.Like);
            var dislikes = reactions.Count(pr => pr.Type == ReactionType.Dislike);

            return (likes, dislikes);
        }
        
        public async Task<IEnumerable<int>> GetPostIdsByUserIdAndReactionTypeAsync(int userId, ReactionType type)
        {
            return await Context.Set<PostReaction>()
                .Where(pr => pr.UserId == userId && pr.Type == type)
                .Select(pr => pr.PostId)
                .ToListAsync();
        }
    }
}