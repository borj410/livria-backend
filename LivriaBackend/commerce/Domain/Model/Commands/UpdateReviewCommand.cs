namespace LivriaBackend.commerce.Domain.Model.Commands
{
    public record UpdateReviewCommand(int ReviewId, int UserClientId, string Content, int Stars);
}