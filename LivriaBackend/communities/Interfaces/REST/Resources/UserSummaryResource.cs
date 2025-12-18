using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    public record UserSummaryResource(
        int UserId,
        string Username,
        string Icon
    );
}