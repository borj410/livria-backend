using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LivriaBackend.communities.Infrastructure.Repositories
{
    public class CommunityRepository : BaseRepository<Community>, ICommunityRepository
    {
        public CommunityRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Community> GetByIdAsync(int id)
        {
            return await Context.Communities
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<IEnumerable<Community>> ListAsync()
        {
            return await Context.Communities
                .ToListAsync();
        }
        
        public async Task AddAsync(Community entity)
        {
            await Context.Set<Community>().AddAsync(entity);
        }
        
        public async Task<bool> ExistsAsync(int id)
        {
            return await Context.Set<Community>().AnyAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Community entity)
        {
            Context.Set<Community>().Update(entity);
            await Task.CompletedTask;
        }
        
        public async Task DeleteAsync(Community entity)
        {
            Context.Set<Community>().Remove(entity);
            await Task.CompletedTask;
        }
        
        /// <summary>
        /// Obtiene una comunidad por su identificador único, incluyendo la colección de miembros (UserCommunities).
        /// </summary>
        public async Task<Community> GetByIdWithMembersAsync(int id)
        {
            return await Context.Communities
                .Include(c => c.UserCommunities)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        
        /// <summary>
        /// Obtiene todas las comunidades donde el usuario especificado es el dueño (OwnerId).
        /// </summary>
        public async Task<IEnumerable<Community>> GetCommunitiesByOwnerIdAsync(int ownerId)
        {
            return await Context.Communities
                .Where(c => c.OwnerId == ownerId)
                .ToListAsync();
        }
    }
}