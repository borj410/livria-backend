namespace LivriaBackend.commerce.Domain.Model.Commands
{
    public record DeleteReviewCommand(int ReviewId, int UserClientId);
}