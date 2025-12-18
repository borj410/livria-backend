using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    public record CommunityMembersDetailsResource(
        int CommunityId,
        UserSummaryResource Owner,
        IEnumerable<UserSummaryResource> Members
    );
}