using System;
using MediatR;
using LivriaBackend.users.Domain.Model.Aggregates;

namespace LivriaBackend.users.Domain.Model.Commands
{
    public record RegisterUserClientCompositeCommand(
        string Username,
        string Password,
        string Display,
        string Email,
        string? Icon,
        string? Phrase
    ) : IRequest<UserClient>;
}