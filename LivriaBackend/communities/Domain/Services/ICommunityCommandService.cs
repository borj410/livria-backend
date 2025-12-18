using LivriaBackend.communities.Domain.Model.Commands;
using LivriaBackend.communities.Domain.Model.Aggregates;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Services
{

    public interface ICommunityCommandService
    {
        Task<Community> Handle(CreateCommunityCommand command);
        Task<Community> Handle(UpdateCommunityCommand command);
        Task<bool> Handle(DeleteCommunityCommand command);
    }
}