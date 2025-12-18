using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Infrastructure.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await Context.Set<Comment>()
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<(int PostId, int AuthorUserId)?> GetPostAndAuthorIdsByIdAsync(int commentId)
        {
            var result = await Context.Set<Comment>()
                .Where(c => c.Id == commentId)
                .Select(c => new { c.PostId, c.UserId }) // Solo selecciona las IDs necesarias
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            return (result.PostId, result.UserId);
        }
        
        public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId)
        {
            return await Context.Comments
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
        
        public async Task AddAsync(Comment entity) => await Context.Set<Comment>().AddAsync(entity);
        public async Task UpdateAsync(Comment entity) => Context.Set<Comment>().Update(entity);
        public async Task DeleteAsync(Comment entity) => Context.Set<Comment>().Remove(entity);
        public override async Task<Comment> GetByIdAsync(int id) => await Context.Set<Comment>().FirstOrDefaultAsync(c => c.Id == id);
    }
}