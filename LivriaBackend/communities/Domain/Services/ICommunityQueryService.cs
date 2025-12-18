using LivriaBackend.communities.Domain.Model.Queries;
using LivriaBackend.communities.Domain.Model.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;
using LivriaBackend.communities.Interfaces.REST.Resources;

namespace LivriaBackend.communities.Domain.Services
{

    public interface ICommunityQueryService
    {
        Task<IEnumerable<Community>> Handle(GetAllCommunitiesQuery query);
        Task<Community> Handle(GetCommunityByIdQuery query);
        Task<bool> Handle(CheckUserJoinedQuery query);
        Task<bool> Handle(CheckUserOwnerQuery query);
        Task<CommunityMembersDetailsResource> Handle(GetCommunityMembersQuery query);
        /// <summary>
        /// Consulta todas las comunidades donde el usuario especificado es el dueño (OwnerId).
        /// </summary>
        Task<IEnumerable<Community>> GetCommunitiesByOwnerIdAsync(int ownerId);
    }
}