using LivriaBackend.communities.Domain.Model.Commands;
using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.shared.Domain.Repositories;
using LivriaBackend.communities.Domain.Services;
using System.Threading.Tasks;
using LivriaBackend.communities.Domain.Model.ValueObjects;
using LivriaBackend.users.Domain.Model.Services;


namespace LivriaBackend.communities.Application.Internal.CommandServices
{
    /// <summary>
    /// Implementa el servicio de comandos para las operaciones de la entidad <see cref="Community"/>.
    /// Encapsula la lógica de negocio para la creación y gestión de comunidades.
    /// </summary>
    public class CommunityCommandService : ICommunityCommandService
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserCommunityCommandService _userCommunityCommandService;
        private readonly IUserClientQueryService _userClientQueryService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CommunityCommandService"/>.
        /// </summary>
        /// <param name="communityRepository">El repositorio para las operaciones de datos de la comunidad.</param>
        /// <param name="unitOfWork">La unidad de trabajo para gestionar las transacciones de base de datos.</param>
        public CommunityCommandService(
            ICommunityRepository communityRepository,
            IUnitOfWork unitOfWork,
            IUserCommunityCommandService userCommunityCommandService,
            IUserClientQueryService userClientQueryService)
        {
            _communityRepository = communityRepository;
            _unitOfWork = unitOfWork;
            _userCommunityCommandService = userCommunityCommandService;
            _userClientQueryService = userClientQueryService;
        }
        
        /// <summary>
        /// Maneja el comando para crear una nueva comunidad.
        /// </summary>
        /// <param name="command">El comando que contiene los datos para crear la comunidad.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Community"/> recién creada.</returns>
        public async Task<Community> Handle(CreateCommunityCommand command)
        {
            var hasPlan = await _userClientQueryService.HasCommunityPlanAsync(command.OwnerId);
            if (!hasPlan)
            {
                throw new ApplicationException("The community owner must have a 'communityplan' subscription to create a community.");
            }
            
            var community = new Community(
                command.Name, 
                command.Description, 
                command.Type, 
                command.OwnerId,
                command.Image, 
                command.Banner
            );
            
            await _communityRepository.AddAsync(community);
            
            await _unitOfWork.CompleteAsync();
            
            if (community.Id == 0)
            {
                throw new ApplicationException("Community could not be created or ID was not generated.");
            }
            
            var joinCommand = new JoinCommunityCommand(
                UserClientId: command.OwnerId,
                CommunityId: community.Id
            );
            
            var userCommunity = await _userCommunityCommandService.Handle(joinCommand);

            if (userCommunity == null)
            {
                throw new ApplicationException("Community created, but owner could not be joined. Possible OwnerId invalid.");
            }
            
            return community;
        }
        
        /// <summary>
        /// Maneja el comando para actualizar una comunidad existente.
        /// Valida que la comunidad exista y que el usuario que intenta actualizar sea el dueño.
        /// </summary>
        /// <param name="command">El comando que contiene los datos actualizados y los IDs de la comunidad y del dueño.</param>
        /// <returns>La comunidad actualizada.</returns>
        /// <exception cref="ArgumentException">Se lanza si la comunidad no existe.</exception>
        /// <exception cref="ApplicationException">Se lanza si el usuario no es el dueño.</exception>
        public async Task<Community> Handle(UpdateCommunityCommand command)
        {
            var community = await _communityRepository.GetByIdAsync(command.CommunityId);
            
            if (community == null)
            {
                throw new ArgumentException($"Community with ID {command.CommunityId} not found.");
            }

            // Validación de propiedad
            if (community.OwnerId != command.UserClientId)
            {
                throw new ApplicationException($"UserClient ID {command.UserClientId} is not the owner of Community ID {command.CommunityId} and cannot update it.");
            }
            
            community.Update(
                command.Name, 
                command.Description, 
                command.Type, 
                command.Image, 
                command.Banner
            );

            await _communityRepository.UpdateAsync(community);
            await _unitOfWork.CompleteAsync();

            return community;
        }
        
        /// <summary>
        /// Maneja el comando para eliminar una comunidad.
        /// Valida que la comunidad exista y que el usuario que intenta eliminar sea el dueño.
        /// </summary>
        /// <param name="command">El comando que contiene los IDs de la comunidad y del dueño.</param>
        /// <returns>True si la eliminación fue exitosa, False si no se encontró la comunidad.</returns>
        /// <exception cref="ApplicationException">Se lanza si el usuario no es el dueño.</exception>
        public async Task<bool> Handle(DeleteCommunityCommand command)
        {
            var community = await _communityRepository.GetByIdAsync(command.CommunityId);
            
            if (community == null)
            {
                return false; // No encontrado, pero la operación de "borrar" se considera "exitosa" si ya no existe.
            }

            // Validación de propiedad
            if (community.OwnerId != command.UserClientId)
            {
                throw new ApplicationException($"UserClient ID {command.UserClientId} is not the owner of Community ID {command.CommunityId} and cannot delete it.");
            }

            await _communityRepository.DeleteAsync(community);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}