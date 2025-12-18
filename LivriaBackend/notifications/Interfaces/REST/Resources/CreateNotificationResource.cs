using System;
using System.ComponentModel.DataAnnotations;
using LivriaBackend.shared.Resources;
using LivriaBackend.shared.Validation;
using LivriaBackend.notifications.Application.Validation;
using LivriaBackend.notifications.Domain.Model.ValueObjects; 

namespace LivriaBackend.notifications.Interfaces.REST.Resources
{
    public record CreateNotificationResource(
        [Required(ErrorMessage = "User Client ID is required.")] 
        int UserClientId,
        
        [Required(ErrorMessage = "EmptyField")]
        string Type, 

        [Required(ErrorMessage = "EmptyField")]
        [DataType(DataType.DateTime)]
        [DateRangeToday(MinimumDate = "1900-01-01", ErrorResourceType = typeof(SharedResource), ErrorResourceName = "DateNotInRange")]
        DateTime CreatedAt
    );
}