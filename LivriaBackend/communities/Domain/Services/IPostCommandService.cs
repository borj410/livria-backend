using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Services
{
    public interface IPostCommandService
    {
        Task<Post> Handle(CreatePostCommand command);
        Task<Post> Handle(UpdatePostCommand command);
        Task<bool> Handle(DeletePostCommand command);
    }
}