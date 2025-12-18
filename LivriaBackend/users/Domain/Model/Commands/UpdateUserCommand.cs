namespace LivriaBackend.users.Domain.Model.Commands
{
    
    public record UpdateUserCommand(
        int UserId,
        string Display,
        string Username,
        string Email
    );
}