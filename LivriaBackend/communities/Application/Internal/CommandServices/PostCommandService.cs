using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.communities.Domain.Services;
using LivriaBackend.shared.Domain.Repositories; 
using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Repositories; 
using System;
using LivriaBackend.communities.Domain.Model.Queries;

namespace LivriaBackend.communities.Application.Internal.CommandServices
{
    /// <summary>
    /// Implementa el servicio de comandos para las operaciones de la entidad <see cref="Post"/>.
    /// Encapsula la lógica de negocio para la creación de publicaciones, incluyendo validaciones de existencia de comunidad y usuario.
    /// </summary>
    public class PostCommandService : IPostCommandService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommunityRepository _communityRepository;
        private readonly IUserClientRepository _userClientRepository; 
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommunityQueryService _communityQueryService;
        private readonly IPostQueryService _postQueryService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostCommandService"/>.
        /// </summary>
        /// <param name="postRepository">El repositorio para las operaciones de datos de publicaciones.</param>
        /// <param name="communityRepository">El repositorio para las operaciones de datos de comunidades.</param>
        /// <param name="userClientRepository">El repositorio para las operaciones de datos de clientes de usuario.</param>
        /// <param name="unitOfWork">La unidad de trabajo para gestionar las transacciones de base de datos.</param>
        public PostCommandService(
            IPostRepository postRepository,
            ICommunityRepository communityRepository,
            IUserClientRepository userClientRepository, 
            IUnitOfWork unitOfWork,
            ICommunityQueryService communityQueryService,
            IPostQueryService postQueryService)
        {
            _postRepository = postRepository;
            _communityRepository = communityRepository;
            _userClientRepository = userClientRepository;
            _unitOfWork = unitOfWork;
            _communityQueryService = communityQueryService;
            _postQueryService = postQueryService;
        }

        /// <summary>
        /// Maneja el comando para crear una nueva publicación.
        /// Valida la existencia de la comunidad y del usuario antes de crear la publicación.
        /// </summary>
        /// <param name="command">El comando que contiene los datos para crear la publicación.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Post"/> recién creada.</returns>
        /// <exception cref="ArgumentException">Se lanza si la comunidad o el cliente de usuario especificados no se encuentran.</exception>
        public async Task<Post> Handle(CreatePostCommand command)
        {
            var community = await _communityRepository.GetByIdAsync(command.CommunityId);
            if (community == null)
            {
                throw new ArgumentException($"Community with ID {command.CommunityId} not found.");
            }
            
            var userClient = await _userClientRepository.GetByIdAsync(command.UserId);
            if (userClient == null)
            {
                throw new ArgumentException($"UserClient with ID '{command.UserId}' not found.");
            }
            
            var post = new Post(
                command.CommunityId, 
                userClient.Id,
                userClient.Username,
                command.Content, 
                command.Img
            );
            
            await _postRepository.AddAsync(post);
            
            await _unitOfWork.CompleteAsync(); 

            return post;
        }
        
        public async Task<Post> Handle(UpdatePostCommand command)
        {
            // 1. Obtener el post
            var post = await _postRepository.GetByIdAsync(command.PostId);
            
            if (post == null)
            {
                throw new ArgumentException($"Post with ID {command.PostId} not found.");
            }
            
            // 2. Validación de Autoría
            bool isAuthor = await _postQueryService.Handle(new CheckPostAuthorQuery(command.PostId, command.UserId));
            
            if (!isAuthor)
            {
                throw new ApplicationException($"User ID {command.UserId} is not the author of Post ID {command.PostId} and cannot edit it.");
            }

            // 3. Actualizar y Persistir
            post.Update(command.Content, command.Img);
            
            await _postRepository.UpdateAsync(post);
            await _unitOfWork.CompleteAsync();

            return post;
        }
        
        public async Task<bool> Handle(DeletePostCommand command)
        {
            // 1. Obtener el post
            var post = await _postRepository.GetByIdAsync(command.PostId);
            
            if (post == null)
            {
                return false; // El post ya no existe
            }
            
            // 2. Validación de Permiso (Autor o Dueño de Comunidad)
            bool isAuthor = post.UserId == command.UserId;
            bool isCommunityOwner = await _communityQueryService.Handle(new CheckUserOwnerQuery(command.UserId, command.CommunityId));
            
            if (!isAuthor && !isCommunityOwner)
            {
                throw new ApplicationException($"User ID {command.UserId} is neither the author of Post ID {command.PostId} nor the owner of Community ID {command.CommunityId} and cannot delete it.");
            }

            // 3. Eliminar y Persistir
            await _postRepository.DeleteAsync(post);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}