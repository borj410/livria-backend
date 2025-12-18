using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;

namespace LivriaBackend.communities.Domain.Services;

public interface IPostReactionCommandService
{
    Task<PostReaction?> Handle(PostReactionCommand command);
}