using LivriaBackend.commerce.Domain.Model.Entities; 
using LivriaBackend.commerce.Domain.Model.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Domain.Model.Services
{
    public interface IRecommendationQueryService
    {
        Task<Recommendation> Handle(GetUserRecommendationsQuery query);
    }
}