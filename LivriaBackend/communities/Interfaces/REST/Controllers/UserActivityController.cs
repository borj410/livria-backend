using AutoMapper;
using LivriaBackend.communities.Application.Internal;
using LivriaBackend.communities.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using LivriaBackend.communities.Domain.Services;

namespace LivriaBackend.communities.Interfaces.REST.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/users/{userId}/activity")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserActivityController : ControllerBase
    {
        private readonly IPostQueryService _postQueryService;
        private readonly ICommunityQueryService _communityQueryService;
        private readonly ICommentQueryService _commentQueryService;
        private readonly IMapper _mapper;

        public UserActivityController(
            IPostQueryService postQueryService,
            ICommunityQueryService communityQueryService,
            ICommentQueryService commentQueryService,
            IMapper mapper)
        {
            _postQueryService = postQueryService;
            _communityQueryService = communityQueryService;
            _commentQueryService = commentQueryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todas las publicaciones creadas por un usuario específico.
        /// </summary>
        [HttpGet("posts")]
        [ProducesResponseType(typeof(IEnumerable<PostResource>), 200)]
        public async Task<IActionResult> GetPostsByUserId(int userId)
        {
            var posts = await _postQueryService.GetPostsByUserIdAsync(userId);
            var postResources = _mapper.Map<IEnumerable<PostResource>>(posts);
            return Ok(postResources);
        }

        /// <summary>
        /// Obtiene todos los comentarios creados por un usuario específico.
        /// </summary>
        [HttpGet("comments")]
        [ProducesResponseType(typeof(IEnumerable<CommentResource>), 200)]
        public async Task<IActionResult> GetCommentsByUserId(int userId)
        {
            var comments = await _commentQueryService.GetCommentsByUserIdAsync(userId);
            var commentResources = _mapper.Map<IEnumerable<CommentResource>>(comments);
            return Ok(commentResources);
        }
        
        /// <summary>
        /// Obtiene todas las comunidades donde el usuario es el dueño (OwnerId).
        /// </summary>
        [HttpGet("owned-communities")]
        [ProducesResponseType(typeof(IEnumerable<CommunityResource>), 200)]
        public async Task<IActionResult> GetOwnedCommunitiesByUserId(int userId)
        {
            var communities = await _communityQueryService.GetCommunitiesByOwnerIdAsync(userId);
            var communityResources = _mapper.Map<IEnumerable<CommunityResource>>(communities);
            return Ok(communityResources);
        }
    }
}