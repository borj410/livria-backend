using LivriaBackend.IAM.Domain.Model.Aggregates;
using System;
using System.Threading.Tasks;

namespace LivriaBackend.IAM.Domain.Repositories
{
    public interface IIdentityRepository
    {
        Task<Identity> GetByIdAsync(int id);
        Task<Identity> GetByUsernameAsync(string username);
        Task AddAsync(Identity identity);
        Task UpdateAsync(Identity identity);
        Task DeleteAsync(Identity identity);

        Task<bool> ExistsByUsernameAsync(string username);
        Task<Identity> GetByUserIdAsync(int userId);
    }
}