using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Queries
{
    public record GetUserRecommendationsQuery(
        [Required] int UserClientId
    );
}