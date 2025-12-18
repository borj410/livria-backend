using AutoMapper;
using LivriaBackend.notifications.Domain.Model.Aggregates;
using LivriaBackend.notifications.Domain.Model.Commands;
using LivriaBackend.notifications.Interfaces.REST.Resources;
using LivriaBackend.notifications.Domain.Model.ValueObjects; 
using System; 

namespace LivriaBackend.notifications.Interfaces.REST.Transform
{
    /// <summary>
    /// Perfil de mapeo de AutoMapper para el módulo de notificaciones.
    /// Define las reglas de transformación entre recursos REST, comandos y agregados/entidades de dominio para las notificaciones.
    /// </summary>
    public class MappingNotification : Profile
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingNotification"/>.
        /// En este constructor, se configuran todas las relaciones de mapeo para el dominio de notificaciones.
        /// </summary>
        public MappingNotification()
        {
     
            CreateMap<CreateNotificationResource, CreateNotificationCommand>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => GetNotificationType(src.Type)));
            
            CreateMap<Notification, NotificationResource>();
        }

        /// <summary>
        /// Convierte una cadena de texto en un valor de la enumeración <see cref="ENotificationType"/>.
        /// La conversión no distingue entre mayúsculas y minúsculas. Si la cadena no puede ser parseada
        /// a un valor válido de <see cref="ENotificationType"/>, se devuelve <see cref="ENotificationType.Default"/>.
        /// </summary>
        /// <param name="typeString">La cadena de texto que representa el tipo de notificación.</param>
        /// <returns>
        /// Un valor de <see cref="ENotificationType"/> correspondiente a la cadena, o <see cref="ENotificationType.Default"/>
        /// si la cadena no es un tipo de notificación válido.
        /// </returns>
        private ENotificationType GetNotificationType(string typeString)
        {
            if (Enum.TryParse(typeString, true, out ENotificationType type))
            {
                return type;
            }
            
            return ENotificationType.Default;
        }
    }
}