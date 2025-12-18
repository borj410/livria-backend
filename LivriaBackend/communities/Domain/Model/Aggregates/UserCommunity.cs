using LivriaBackend.users.Domain.Model.Aggregates; 
using LivriaBackend.communities.Domain.Model.Aggregates; 
using System;

namespace LivriaBackend.communities.Domain.Model.Aggregates
{
    /// <summary>
    /// Representa la relación de membresía entre un <see cref="UserClient"/> y una <see cref="Community"/>.
    /// Esta entidad agregada modela la participación de un usuario en una comunidad específica.
    /// </summary>
    public class UserCommunity
    {
        /// <summary>
        /// Obtiene el identificador del cliente de usuario.
        /// Esta es parte de la clave compuesta de la relación.
        /// </summary>
        public int UserClientId { get; private set; } 

        /// <summary>
        /// Propiedad de navegación al <see cref="UserClient"/> asociado con esta membresía.
        /// </summary>
        public UserClient UserClient { get; private set; } 

        /// <summary>
        /// Obtiene el identificador de la comunidad.
        /// Esta es parte de la clave compuesta de la relación.
        /// </summary>
        public int CommunityId { get; private set; } 

        /// <summary>
        /// Propiedad de navegación a la <see cref="Community"/> asociada con esta membresía.
        /// </summary>
        public Community Community { get; private set; } 

        /// <summary>
        /// Obtiene la fecha y hora en que el usuario se unió a la comunidad en formato UTC.
        /// </summary>
        public DateTime JoinedDate { get; private set; }

        /// <summary>
        /// Constructor privado sin parámetros para uso de Entity Framework Core.
        /// </summary>
        private UserCommunity() { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserCommunity"/> con los identificadores especificados.
        /// La fecha de unión se establece automáticamente a la hora UTC actual.
        /// </summary>
        /// <param name="userClientId">El identificador del cliente de usuario que se une a la comunidad.</param>
        /// <param name="communityId">El identificador de la comunidad a la que se une el usuario.</param>
        public UserCommunity(int userClientId, int communityId)
        {
            UserClientId = userClientId;
            CommunityId = communityId;
            JoinedDate = DateTime.UtcNow; 
        }
    }
}