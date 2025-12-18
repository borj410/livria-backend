using AutoMapper;
using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Services;
using LivriaBackend.communities.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mime; 
using System;
using LivriaBackend.communities.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace LivriaBackend.communities.Interfaces.REST.Controllers
{
    /// <summary>
    /// Controlador RESTful para gestionar las operaciones relacionadas con las publicaciones (posts).
    /// </summary>
    [Authorize(Roles = "UserClient,Admin")]
    [ApiController]
    [Route("api/v1/posts")] 
    [Produces(MediaTypeNames.Application.Json)]
    public class PostController : ControllerBase
    {
        private readonly IPostCommandService _postCommandService;
        private readonly IPostQueryService _postQueryService;
        private readonly IPostReactionCommandService _postReactionCommandService;
        private readonly IPostReactionQueryService _postReactionQueryService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostController"/>.
        /// </summary>
        /// <param name="postCommandService">El servicio de comandos de publicaciones.</param>
        /// <param name="postQueryService">El servicio de consulta de publicaciones.</param>
        /// <param name="mapper">La instancia de AutoMapper para la transformación de objetos.</param>
        public PostController(IPostCommandService postCommandService, IPostQueryService postQueryService, IPostReactionCommandService postReactionCommandService, IPostReactionQueryService postReactionQueryService, IMapper mapper)
        {
            _postCommandService = postCommandService;
            _postQueryService = postQueryService;
            _postReactionCommandService = postReactionCommandService;
            _postReactionQueryService = postReactionQueryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Crea una nueva publicación en una comunidad existente.
        /// </summary>
        /// <param name="communityId">El identificador único de la comunidad a la que pertenecerá la publicación.</param>
        /// <param name="resource">El recurso que contiene los datos de la publicación a crear.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="PostResource"/> de la publicación creada
        /// con un código 201 CreatedAtAction si la operación es exitosa.
        /// Retorna BadRequest (400) si la publicación no pudo ser creada o si la comunidad/usuario no existen.
        /// </returns>
        [HttpPost("communities/{communityId}")] 
        [SwaggerOperation(
            Summary= "Crear una nueva publicación en una comunidad existente.",
            Description= "Crea una nueva publicación en una comunidad existente en el sistema."
        )]
        [ProducesResponseType(typeof(PostResource), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PostResource>> CreatePost(int communityId, [FromBody] CreatePostResource resource)
        {
            var createPostCommand = new CreatePostCommand(
                communityId,
                resource.UserId, 
                resource.Content,
                resource.Img
            );

            Post post;
            try
            {
                post = await _postCommandService.Handle(createPostCommand);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
            
            if (post == null)
            {
                // Este caso debería ser cubierto por las excepciones en el servicio, pero se mantiene como un fallback.
                return BadRequest(new { message = "Could not create post." });
            }

            var postResource = _mapper.Map<PostResource>(post);
            
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, postResource);
        }

        /// <summary>
        /// Obtiene los datos de una publicación específica por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la publicación.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un <see cref="PostResource"/> si la publicación es encontrada (código 200 OK),
        /// o un resultado NotFound (código 404) si la publicación no existe.
        /// </returns>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary= "Obtener los datos de una publicación en específico.",
            Description= "Te muestra los datos de la publicación que buscaste."
        )]
        [ProducesResponseType(typeof(PostResource), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PostResource>> GetPostById(int id)
        {
            var post = await _postQueryService.Handle(new GetPostByIdQuery(id));
            if (post == null)
            {
                return NotFound();
            }
            var postResource = _mapper.Map<PostResource>(post);
            return Ok(postResource);
        }

        /// <summary>
        /// Obtiene los datos de todas las publicaciones disponibles en el sistema.
        /// </summary>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="PostResource"/>
        /// si la operación es exitosa (código 200 OK). Puede ser una colección vacía si no hay publicaciones.
        /// </returns>
        [HttpGet]
        [SwaggerOperation(
            Summary= "Obtener los datos de todas las publicaciones.",
            Description= "Te muestra los datos de las publicaciones."
            
        )]
        [ProducesResponseType(typeof(IEnumerable<PostResource>), 200)]
        public async Task<ActionResult<IEnumerable<PostResource>>> GetAllPosts()
        {
            var posts = await _postQueryService.Handle(new GetAllPostsQuery());
            var postResources = _mapper.Map<IEnumerable<PostResource>>(posts);
            return Ok(postResources);
        }
        
        /// <summary>
        /// Obtiene todas las publicaciones para una comunidad específica por su identificador único.
        /// </summary>
        /// <param name="communityId">El identificador único de la comunidad.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="PostResource"/>
        /// para la comunidad especificada (código 200 OK). Puede ser una colección vacía si la comunidad no tiene publicaciones.
        /// </returns>
        [HttpGet("community/{communityId}")] 
        [SwaggerOperation(
            Summary = "Obtener todas las publicaciones para una comunidad específica.",
            Description = "Obtiene una lista de todas las publicaciones asociadas con un ID de comunidad dado."
        )]
        [ProducesResponseType(typeof(IEnumerable<PostResource>), 200)]
        public async Task<ActionResult<IEnumerable<PostResource>>> GetPostsByCommunityId(int communityId)
        {
            var query = new GetPostsByCommunityIdQuery(communityId);
            var posts = await _postQueryService.Handle(query);

            if (posts == null) 
            {
                return Ok(Enumerable.Empty<PostResource>()); 
            }

            var resources = _mapper.Map<IEnumerable<PostResource>>(posts);
            return Ok(resources);
        }
        
        /// <summary>
        /// Actualiza el contenido y la imagen de una publicación existente.
        /// Solo el autor original del post puede realizar esta operación.
        /// </summary>
        /// <param name="id">El identificador único de la publicación a actualizar.</param>
        /// <param name="resource">El recurso que contiene el nuevo contenido y la imagen, incluyendo el ID del autor.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="PostResource"/> actualizado (código 200 OK).
        /// Retorna NotFound (404) si la publicación no existe.
        /// Retorna BadRequest (400) si el usuario no es el autor o si los datos son inválidos.
        /// </returns>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar una publicación existente.",
            Description = "Modifica el contenido de una publicación. Solo permitido al autor."
        )]
        [ProducesResponseType(typeof(PostResource), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PostResource>> UpdatePost(int id, [FromBody] UpdatePostResource resource)
        {
            var command = new UpdatePostCommand(
                PostId: id,
                UserId: resource.UserId,
                Content: resource.Content,
                Img: resource.Img ?? string.Empty
            );

            try
            {
                var post = await _postCommandService.Handle(command);
                var postResource = _mapper.Map<PostResource>(post);
                return Ok(postResource);
            }
            catch (ArgumentException ex) when (ex.Message.Contains("not found"))
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ApplicationException ex) when (ex.Message.Contains("not the author"))
            {
                // Usamos 403 Forbidden para permisos, o 400 Bad Request si el controlador lo prefiere
                return BadRequest(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
            }
        }
        
        /// <summary>
        /// Elimina una publicación existente.
        /// Permitido al autor del post o al dueño de la comunidad.
        /// </summary>
        /// <param name="id">El identificador único de la publicación a eliminar.</param>
        /// <param name="resource">El recurso que contiene el ID del usuario que intenta eliminar y el ID de la comunidad.</param>
        /// <returns>
        /// Un No Content (204) si la eliminación fue exitosa.
        /// Retorna NotFound (404) si la publicación no existe.
        /// Retorna BadRequest (400) si el usuario no tiene permiso.
        /// </returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar una publicación existente.",
            Description = "Elimina una publicación. Permitido al autor o al dueño de la comunidad."
        )]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeletePost(int id, [FromBody] DeletePostResource resource)
        {
            var command = new DeletePostCommand(
                PostId: id,
                UserId: resource.UserId,
                CommunityId: resource.CommunityId
            );

            try
            {
                bool result = await _postCommandService.Handle(command);
                
                if (!result)
                {
                    return NotFound(new { message = $"Post with ID {id} not found." });
                }

                return NoContent();
            }
            catch (ApplicationException ex) when (ex.Message.Contains("not the author") || ex.Message.Contains("not the owner"))
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
            }
        }
        
        /// <summary>
        /// Verifica si un cliente de usuario específico es el autor de una publicación.
        /// </summary>
        /// <param name="postId">El ID de la publicación.</param>
        /// <param name="userId">El ID del cliente de usuario cuya autoría se verifica.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un objeto JSON con el campo 'isAuthor' (bool) (código 200 OK).
        /// </returns>
        [HttpGet("{postId}/author/{userId}/is-author")]
        [SwaggerOperation(
            Summary= "Verificar la autoría de un usuario en un post.",
            Description= "Comprueba si el cliente de usuario especificado es el autor (creador) del post."
        )]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<ActionResult<object>> CheckPostAuthor(int postId, int userId)
        {
            var query = new CheckPostAuthorQuery(postId, userId);
            var isAuthor = await _postQueryService.Handle(query);

            return Ok(new { isAuthor = isAuthor });
        }
        
        /// <summary>
        /// Permite a un usuario registrar, actualizar o eliminar una reacción (Like/Dislike) a una publicación.
        /// </summary>
        /// <param name="postId">El identificador único de la publicación a reaccionar.</param>
        /// <param name="resource">El recurso que contiene el ID del usuario y el tipo de reacción deseado (1: Like, 2: Dislike, 0: Eliminar).</param>
        /// <returns>
        /// Un código 201 Created si la reacción es nueva o actualizada.
        /// Un código 204 No Content si la reacción fue eliminada.
        /// Retorna BadRequest (400) si la publicación no existe o los datos son inválidos.
        /// </returns>
        [HttpPost("{postId}/reactions")]
        [SwaggerOperation(
            Summary = "Registrar, actualizar o eliminar una reacción a un post.",
            Description = "Permite a un usuario dar Like, Dislike o remover su reacción de un post."
        )]
        [ProducesResponseType(typeof(PostReactionResource), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> ReactToPost(int postId, [FromBody] ReactToPostResource resource)
        {
            var command = new PostReactionCommand(
                UserId: resource.UserId,
                PostId: postId,
                Type: resource.Type
            );

            try
            {
                var reaction = await _postReactionCommandService.Handle(command);

                if (reaction == null)
                {
                    // Si el servicio devuelve null, significa que la reacción fue eliminada
                    return NoContent(); // 204 No Content
                }

                // Aquí necesitarás un PostReactionResource y un mapeo
                // Asumo que existe un PostReactionResource que mapea PostReaction
                var reactionResource = _mapper.Map<PostReactionResource>(reaction); 
                
                // 201 Created/OK si la reacción se creó o actualizó
                return Created(string.Empty, reactionResource); 
            }
            catch (ArgumentException ex) when (ex.Message.Contains("Post with ID"))
            {
                // El post no existe
                return NotFound(new { message = ex.Message }); 
            }
            catch (ArgumentException ex) 
            {
                // Otra validación (ej. tipo de reacción inválido si se agregó esa lógica)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
            }
        }
        
        /// <summary>
        /// Obtiene el conteo de likes y dislikes para una publicación específica.
        /// </summary>
        [HttpGet("{postId}/reactions/counts")]
        [SwaggerOperation(Summary = "Obtener el conteo de reacciones (Likes y Dislikes) de un post.")]
        [ProducesResponseType(typeof(ReactionCountsResource), 200)]
        public async Task<ActionResult<ReactionCountsResource>> GetPostReactionCounts(int postId)
        {
            var query = new GetReactionCountsQuery(postId);
            var (likes, dislikes) = await _postReactionQueryService.Handle(query);

            var resource = new ReactionCountsResource(likes, dislikes);
            return Ok(resource);
        }
        
        /// <summary>
        /// Obtiene el estado de la reacción (Like, Dislike, None) de un usuario en un post.
        /// </summary>
        [HttpGet("{postId}/user/{userId}/reaction-status")]
        [SwaggerOperation(Summary = "Verificar si un usuario ha reaccionado a un post y con qué tipo.")]
        [ProducesResponseType(typeof(UserReactionStatusResource), 200)]
        public async Task<ActionResult<UserReactionStatusResource>> GetUserReactionStatus(int postId, int userId)
        {
            var query = new GetUserReactionStatusQuery(userId, postId);
            var reactionType = await _postReactionQueryService.Handle(query);

            var resource = new UserReactionStatusResource(reactionType);
            return Ok(resource);
        }
        
        /// <summary>
        /// Obtiene la lista de publicaciones a las que un usuario le ha dado Like.
        /// </summary>
        [HttpGet("user/{userId}/liked")]
        [SwaggerOperation(Summary = "Obtener todos los posts a los que un usuario le dio Like.")]
        [ProducesResponseType(typeof(IEnumerable<PostResource>), 200)]
        public async Task<ActionResult<IEnumerable<PostResource>>> GetLikedPosts(int userId)
        {
            var query = new GetPostsByReactionTypeQuery(userId, ReactionType.Like);
            var posts = await _postReactionQueryService.Handle(query);
            
            var resources = _mapper.Map<IEnumerable<PostResource>>(posts);
            return Ok(resources);
        }
        
        /// <summary>
        /// Obtiene la lista de publicaciones a las que un usuario le ha dado Dislike.
        /// </summary>
        [HttpGet("user/{userId}/disliked")]
        [SwaggerOperation(Summary = "Obtener todos los posts a los que un usuario le dio Dislike.")]
        [ProducesResponseType(typeof(IEnumerable<PostResource>), 200)]
        public async Task<ActionResult<IEnumerable<PostResource>>> GetDislikedPosts(int userId)
        {
            var query = new GetPostsByReactionTypeQuery(userId, ReactionType.Dislike);
            var posts = await _postReactionQueryService.Handle(query);
            
            var resources = _mapper.Map<IEnumerable<PostResource>>(posts);
            return Ok(resources);
        }
    }
}