using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LivriaBackend.communities.Domain.Model.ValueObjects; 

namespace LivriaBackend.communities.Interfaces.REST.Resources
{
    public record CommunityResource(
        int Id,
        
        [StringLength(100, ErrorMessage = "MaxLengthError")]
        string Name,
        
        [StringLength(500, ErrorMessage = "MaxLengthError")]
        string Description,
        
        CommunityType Type,
        
        int OwnerId,
        
        string Image,
        
        string Banner
    );
}