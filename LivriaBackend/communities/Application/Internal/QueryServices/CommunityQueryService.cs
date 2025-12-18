using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Services;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.users.Domain.Model.Services;
using LivriaBackend.users.Domain.Model.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;
using LivriaBackend.communities.Interfaces.REST.Resources;

namespace LivriaBackend.communities.Application.Internal.QueryServices
{
    /// <summary>
    /// Implementa el servicio de consulta para las operaciones de la entidad <see cref="Community"/>.
    /// Encapsula la lógica de negocio para recuperar datos de comunidades.
    /// </summary>
    public class CommunityQueryService : ICommunityQueryService
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly IUserCommunityRepository _userCommunityRepository;
        private readonly IUserClientQueryService _userClientQueryService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CommunityQueryService"/>.
        /// </summary>
        /// <param name="communityRepository">El repositorio para las operaciones de datos de la comunidad.</param>
        /// <param name="userCommunityRepository">El repositorio para las operaciones de datos de la relación UserCommunity.</param>
        public CommunityQueryService(
            ICommunityRepository communityRepository,
            IUserCommunityRepository userCommunityRepository,
            IUserClientQueryService userClientQueryService)
        {
            _communityRepository = communityRepository;
            _userCommunityRepository = userCommunityRepository;
            _userClientQueryService = userClientQueryService;
        }

        /// <summary>
        /// Maneja el comando para obtener todas las comunidades.
        /// </summary>
        /// <param name="query">La consulta para obtener todas las comunidades.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de todas las <see cref="Community"/>.</returns>
        public async Task<IEnumerable<Community>> Handle(GetAllCommunitiesQuery query)
        {
            return await _communityRepository.ListAsync();
        }

        /// <summary>
        /// Maneja el comando para obtener una comunidad por su identificador único.
        /// </summary>
        /// <param name="query">La consulta para obtener una comunidad por ID.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Community"/> encontrada, o null si no existe.</returns>
        public async Task<Community> Handle(GetCommunityByIdQuery query)
        {
            return await _communityRepository.GetByIdAsync(query.CommunityId);
        }
        
        /// <summary>
        /// Maneja la consulta para verificar si un usuario es miembro de una comunidad específica.
        /// </summary>
        /// <param name="query">La consulta que contiene los IDs del usuario y la comunidad.</param>
        /// <returns>True si el usuario es miembro de la comunidad; de lo contrario, False.</returns>
        public async Task<bool> Handle(CheckUserJoinedQuery query)
        {
            // Usamos el repositorio inyectado para la relación (UserCommunity)
            var userCommunity = await _userCommunityRepository
                .GetByUserAndCommunityIdsAsync(query.UserClientId, query.CommunityId);

            // Si el registro existe (no es null), el usuario está unido.
            return userCommunity != null;
        }
        
        /// <summary>
        /// Maneja la consulta para verificar si un usuario es dueño de una comunidad específica.
        /// </summary>
        /// <param name="query">La consulta que contiene los IDs del usuario y la comunidad.</param>
        /// <returns>True si el usuario es el dueño de la comunidad; de lo contrario, False.</returns>
        public async Task<bool> Handle(CheckUserOwnerQuery query)
        {
            var community = await _communityRepository.GetByIdAsync(query.CommunityId);
            
            if (community == null)
            {
                return false;
            }
            
            return community.OwnerId == query.UserClientId;
        }
        
        public async Task<CommunityMembersDetailsResource> Handle(GetCommunityMembersQuery query) 
        {
            var community = await _communityRepository.GetByIdWithMembersAsync(query.CommunityId); 
            
            if (community == null)
            {
                return null; 
            }
            
            var memberIds = community.UserCommunities
                .Select(uc => uc.UserClientId)
                .Where(userId => userId != community.OwnerId)
                .ToList();
            
            var memberResources = new List<UserSummaryResource>();
            UserSummaryResource ownerResource = null;
            
            var ownerClient = await _userClientQueryService.Handle(new GetUserClientByIdQuery(community.OwnerId));
            
            if (ownerClient != null)
            {
                ownerResource = new UserSummaryResource(ownerClient.Id, ownerClient.Username, ownerClient.Icon);
            } 
            else 
            {
                 throw new InvalidOperationException($"Owner with ID {community.OwnerId} for community {community.Id} not found.");
            }
            
            foreach (var userId in memberIds)
            {
                var userClient = await _userClientQueryService.Handle(new GetUserClientByIdQuery(userId));

                if (userClient != null)
                {
                    var userResource = new UserSummaryResource(
                        userClient.Id,
                        userClient.Username, 
                        userClient.Icon      
                    );
                    memberResources.Add(userResource);
                }
            }
            
            return new CommunityMembersDetailsResource(
                community.Id,
                ownerResource,
                memberResources
            );
        }
        
        public async Task<IEnumerable<Community>> GetCommunitiesByOwnerIdAsync(int ownerId)
        {
            return await _communityRepository.GetCommunitiesByOwnerIdAsync(ownerId);
        }
    }
}