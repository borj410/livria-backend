using System.Collections.Generic;
using LivriaBackend.users.Domain.Model.Aggregates;
using System.Linq;
using LivriaBackend.communities.Domain.Model.ValueObjects; 

namespace LivriaBackend.communities.Domain.Model.Aggregates
{
    /// <summary>
    /// Representa un agregado de comunidad dentro del dominio.
    /// Una comunidad agrupa usuarios que la integran.
    /// </summary>
    public class Community
    {
        /// <summary>
        /// Obtiene el identificador único de la comunidad.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Obtiene el nombre de la comunidad.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Obtiene la descripción de la comunidad.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Obtiene el identificador del usuario que creó la comunidad.
        /// </summary>
        public int OwnerId { get; private set; }     
        
        /// <summary>
        /// Obtiene el tipo de comunidad (género literario).
        /// </summary>
        public CommunityType Type { get; private set; } 

        /// <summary>
        /// Obtiene la URL de la imagen de perfil de la comunidad.
        /// </summary>
        public string Image { get; private set; }

        /// <summary>
        /// Obtiene la URL de la imagen de banner de la comunidad.
        /// </summary>
        public string Banner { get; private set; }


        /// <summary>
        /// Obtiene una colección de las relaciones entre usuarios y esta comunidad (miembros).
        /// </summary>
        public ICollection<UserCommunity> UserCommunities { get; private set; } = new List<UserCommunity>();

        /// <summary>
        /// Constructor privado sin parámetros para uso de Entity Framework Core.
        /// Inicializa las colecciones de relaciones de usuarios con la comunidad.
        /// </summary>
        private Community()
        {
            UserCommunities = new List<UserCommunity>();
            Name = string.Empty;
            Description = string.Empty;
            Image = string.Empty;
            Banner = string.Empty;
            OwnerId = 0;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Community"/> con los detalles especificados.
        /// </summary>
        /// <param name="name">El nombre de la comunidad.</param>
        /// <param name="description">La descripción de la comunidad.</param>
        /// <param name="type">El tipo de comunidad (género literario).</param>
        /// <param name="image">La URL de la imagen de perfil de la comunidad. Opcional, por defecto string.Empty.</param>
        /// <param name="banner">La URL del banner de la comunidad. Opcional, por defecto string.Empty.</param>
        public Community(string name, string description, CommunityType type, int ownerId, string? image = null, string? banner = null) 
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description cannot be empty.", nameof(description));

            Name = name;
            Description = description;
            Type = type;
            OwnerId = ownerId;
            Image = image ?? string.Empty; 
            Banner = banner ?? string.Empty;
            UserCommunities = new List<UserCommunity>();
        }

        
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentException("Name cannot be empty.", nameof(newName));
            Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription)) throw new ArgumentException("Description cannot be empty.", nameof(newDescription));
            Description = newDescription;
        }

        public void UpdateType(CommunityType newType) 
        {
            Type = newType;
        }

        public void UpdateImage(string? newImage)
        {
            Image = newImage ?? string.Empty;
        }

        public void UpdateBanner(string? newBanner)
        {
            Banner = newBanner ?? string.Empty;
        }

        /// <summary>
        /// Consolida la actualización de todos los atributos modificables de la comunidad.
        /// </summary>
        /// <param name="newName">El nuevo nombre de la comunidad.</param>
        /// <param name="newDescription">La nueva descripción de la comunidad.</param>
        /// <param name="newType">El nuevo tipo de comunidad (género literario).</param>
        /// <param name="newImage">La nueva URL de la imagen de perfil de la comunidad..</param>
        /// <param name="newBanner">La nueva URL del banner de la comunidad..</param>
        public void Update(string newName, string newDescription, CommunityType newType, string newImage, string newBanner)
        {
            UpdateName(newName);
            UpdateDescription(newDescription);
            UpdateType(newType);
            UpdateImage(newImage);
            UpdateBanner(newBanner);
        }

        /// <summary>
        /// Añade un cliente de usuario como miembro de esta comunidad creando una relación <see cref="UserCommunity"/>.
        /// </summary>
        /// <param name="userClient">El cliente de usuario a añadir. Debe ser un objeto <see cref="UserClient"/> válido y no debe ser ya miembro de la comunidad.</param>
        public void AddUser(UserClient userClient)
        {
            if (userClient == null)
            {
                throw new ArgumentNullException(nameof(userClient), "UserClient cannot be null.");
            }
            if (UserCommunities.Any(uc => uc.UserClientId == userClient.Id))
            {
                return;
            }
            UserCommunities.Add(new UserCommunity(userClient.Id, this.Id));
        }
        
        /// <summary>
        /// Anonimiza el dueño de la comunidad asignándolo al usuario eliminado.
        /// </summary>
        public void AnonymizeOwner(int deletedUserId)
        {
            // Asumiendo que el UserConstants.DeletedUserId es la ID correcta (2)
            OwnerId = deletedUserId;
        }
    }
}