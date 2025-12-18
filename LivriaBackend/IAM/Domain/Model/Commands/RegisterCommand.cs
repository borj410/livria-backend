using MediatR;
namespace LivriaBackend.IAM.Domain.Model.Commands
{
    public record RegisterCommand(
        int UserId,
        string Username,
        string Password): IRequest<int>;
}