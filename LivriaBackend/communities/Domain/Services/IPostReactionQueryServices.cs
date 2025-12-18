using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Model.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Services
{
    public interface IPostReactionQueryService
    {
        Task<(int Likes, int Dislikes)> Handle(GetReactionCountsQuery query); 
        Task<ReactionType> Handle(GetUserReactionStatusQuery query);
        Task<IEnumerable<Post>> Handle(GetPostsByReactionTypeQuery query);
    }
}