using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Queries
{
    public record GetOrdersByUserIdQuery(
        [Required] int UserClientId
    );
}