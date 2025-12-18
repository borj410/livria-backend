using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Services
{
    public interface ICommentCommandService
    {
        Task<Comment> Handle(CreateCommentCommand command);
        Task<bool> Handle(DeleteCommentCommand command);
    }
}