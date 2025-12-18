using LivriaBackend.users.Domain.Model.Aggregates; 
using System;

namespace LivriaBackend.communities.Domain.Model.Aggregates
{
    /// <summary>
    /// Representa una publicación (Post) como una entidad agregada dentro del dominio de comunidades.
    /// Un Post pertenece a una <see cref="Community"/> y es creado por un <see cref="UserClient"/>.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Obtiene el identificador único de la publicación.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Obtiene el identificador de la comunidad a la que pertenece esta publicación.
        /// </summary>
        public int CommunityId { get; private set; } 

        /// <summary>
        /// Obtiene el identificador del usuario que creó esta publicación.
        /// </summary>
        public int UserId { get; private set; }      

        /// <summary>
        /// Obtiene el nombre de usuario de la persona que realizó la publicación.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Obtiene el contenido de texto de la publicación.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Obtiene la URL de la imagen asociada a la publicación, si la hay.
        /// </summary>
        public string Img { get; private set; }

        /// <summary>
        /// Obtiene la fecha y hora de creación de la publicación en formato UTC.
        /// </summary>
        public DateTime CreatedAt { get; private set; } 

        /// <summary>
        /// Propiedad de navegación a la <see cref="Community"/> a la que pertenece esta publicación.
        /// </summary>
        public Community Community { get; private set; }

        /// <summary>
        /// Propiedad de navegación al <see cref="UserClient"/> que creó esta publicación.
        /// </summary>
        public UserClient UserClient { get; private set; } 

        /// <summary>
        /// Constructor protegido sin parámetros para uso de Entity Framework Core.
        /// </summary>
        protected Post() { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Post"/> con los detalles especificados.
        /// La fecha de creación se establece automáticamente a la hora UTC actual.
        /// </summary>
        /// <param name="communityId">El identificador de la comunidad a la que pertenece la publicación.</param>
        /// <param name="userId">El identificador del usuario que crea la publicación.</param>
        /// <param name="username">El nombre de usuario del autor de la publicación.</param>
        /// <param name="content">El contenido de texto de la publicación.</param>
        /// <param name="img">La URL de la imagen de la publicación. Puede ser nula o vacía.</param>
        public Post(int communityId, int userId, string username, string content, string img)
        {
            CommunityId = communityId;
            UserId = userId;
            Username = username;
            Content = content;
            Img = img;
            CreatedAt = DateTime.UtcNow; 
        }

        /// <summary>
        /// Actualiza el contenido y la imagen de una publicación existente.
        /// </summary>
        /// <param name="content">El nuevo contenido de texto para la publicación.</param>
        /// <param name="img">La nueva URL de la imagen para la publicación.</param>
        public void Update(string content, string img)
        {
            Content = content;
            Img = img;
        }
        
        /// <summary>
        /// Anonimiza la publicación asignándola al usuario eliminado.
        /// </summary>
        public void Anonymize()
        {
            UserId = UserConstants.DeletedUserId;
            Username = UserConstants.DeletedUsername;
        }
    }
}