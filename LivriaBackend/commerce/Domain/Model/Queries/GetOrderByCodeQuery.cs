using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Queries
{
    public record GetOrderByCodeQuery(
        [Required] [StringLength(6)] string OrderCode
    );
}