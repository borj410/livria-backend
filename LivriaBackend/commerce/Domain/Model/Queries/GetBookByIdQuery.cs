using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.Queries
{
    public record GetBookByIdQuery(
        [Required] int BookId
    );
}