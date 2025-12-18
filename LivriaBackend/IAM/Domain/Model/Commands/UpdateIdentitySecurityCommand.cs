using MediatR;

namespace LivriaBackend.IAM.Domain.Model.Commands
{
    public record UpdateIdentitySecurityCommand(
        int UserId, 
        string CurrentPassword, 
        string? NewUsername, 
        string? NewPassword) : IRequest<bool>;
}