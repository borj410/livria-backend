using System;
using System.ComponentModel.DataAnnotations;
using LivriaBackend.notifications.Application.Validation; 
using LivriaBackend.notifications.Domain.Model.ValueObjects;
using LivriaBackend.shared.Resources;
using LivriaBackend.shared.Validation;

namespace LivriaBackend.notifications.Interfaces.REST.Resources
{
    public record NotificationResource(
        int Id,
        int UserClientId, 
        
        [DataType(DataType.DateTime)]
        
        [DateRangeToday(MinimumDate = "1900-01-01", ErrorResourceType = typeof(SharedResource), ErrorResourceName = "DateNotInRange")]
        DateTime CreatedAt, 
        
        [ValidNotificationType(ErrorResourceType = typeof(LivriaBackend.notifications.Application.Resources.DataAnnotations), ErrorResourceName = "InvalidNotificationType")]
        ENotificationType Type, 
        
        [StringLength(255, ErrorMessage = "MaxLengthError")]
        string Title, 
        
        [StringLength(500, ErrorMessage = "MaxLengthError")]
        string Content, 

        bool IsRead,    
        bool IsHidden   
    );
}