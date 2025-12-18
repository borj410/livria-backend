using MediatR;
using LivriaBackend.IAM.Application.Resources;

namespace LivriaBackend.IAM.Domain.Model.Commands
{
    public record LoginAdminCommand(
        string Username,
        string Password,
        string SecurityPin) : IRequest<LoginResponse>;
}