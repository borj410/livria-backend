using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Queries
{
    public record GetOrderByIdQuery(
        [Required] int OrderId
    );
}