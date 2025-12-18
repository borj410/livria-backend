using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Queries
{
    public record GetCartItemByIdQuery(
        [Required] int CartItemId
    );
}