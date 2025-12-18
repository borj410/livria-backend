using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Queries
{
    public record GetAllCartItemsByUserIdQuery(
        [Required] int UserClientId
    );
}