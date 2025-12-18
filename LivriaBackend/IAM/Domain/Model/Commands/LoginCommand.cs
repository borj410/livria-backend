using MediatR;
using LivriaBackend.IAM.Application.Resources;

namespace LivriaBackend.IAM.Domain.Model.Commands
{
    public record LoginCommand(
        string Username,
        string Password): IRequest<LoginResponse>;
}