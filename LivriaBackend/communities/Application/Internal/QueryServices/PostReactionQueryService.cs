using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Services;
using LivriaBackend.communities.Domain.Model.ValueObjects;
using LivriaBackend.communities.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Application.Internal.QueryServices
{
    public class PostReactionQueryService : IPostReactionQueryService
    {
        private readonly IPostReactionRepository _postReactionRepository;
        private readonly IPostRepository _postRepository;

        public PostReactionQueryService(IPostReactionRepository postReactionRepository, IPostRepository postRepository)
        {
            _postReactionRepository = postReactionRepository;
            _postRepository = postRepository;
        }
        
        public async Task<(int Likes, int Dislikes)> Handle(GetReactionCountsQuery query)
        {
            return await _postReactionRepository.GetReactionCountsByPostIdAsync(query.PostId);
        }
        
        public async Task<ReactionType> Handle(GetUserReactionStatusQuery query)
        {
            var reaction = await _postReactionRepository.GetByUserIdAndPostIdAsync(query.UserId, query.PostId);
            
            return reaction?.Type ?? ReactionType.None;
        }
        
        public async Task<IEnumerable<Post>> Handle(GetPostsByReactionTypeQuery query)
        {
            var postIds = await _postReactionRepository.GetPostIdsByUserIdAndReactionTypeAsync(query.UserId, query.Type);

            if (!postIds.Any())
            {
                return Enumerable.Empty<Post>();
            }
            
            var posts = new List<Post>();
            foreach (var id in postIds)
            {
                var post = await _postRepository.GetByIdAsync(id); 
                if (post != null)
                {
                    posts.Add(post);
                }
            }

            return posts;
        }
    }
}