using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.users.Domain.Model.Aggregates; 

namespace LivriaBackend.commerce.Domain.Model.Entities
{
    /// <summary>
    /// Representa una reseña de un libro. Es una entidad de dominio.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Obtiene el identificador único de la reseña.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Obtiene el nombre de usuario asociado a la reseña.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Obtiene el contenido textual de la reseña.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Obtiene la calificación por estrellas de la reseña.
        /// </summary>
        public int Stars { get; private set; }

        /// <summary>
        /// Obtiene el identificador único del libro al que se refiere la reseña.
        /// </summary>
        public int BookId { get; private set; }

        /// <summary>
        /// Obtiene el objeto <see cref="Book"/> al que se refiere la reseña.
        /// </summary>
        public Book Book { get; private set; } 
        
        /// <summary>
        /// Obtiene el identificador único del cliente de usuario que escribió la reseña.
        /// </summary>
        public int UserClientId { get; private set; } 

        /// <summary>
        /// Obtiene el objeto <see cref="UserClient"/> que escribió la reseña.
        /// </summary>
        public UserClient UserClient { get; private set; } 

        /// <summary>
        /// Constructor protegido para uso de frameworks ORM (como Entity Framework Core).
        /// No debe ser utilizado directamente para la creación de instancias de <see cref="Review"/>.
        /// </summary>
        protected Review() { }
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Review"/>.
        /// </summary>
        /// <param name="bookId">El identificador del libro.</param>
        /// <param name="userClientId">El identificador del cliente de usuario.</param>
        /// <param name="content">El contenido de la reseña.</param>
        /// <param name="stars">La calificación de estrellas.</param>
        /// <param name="username">El nombre de usuario para mostrar en la reseña.</param>
        public Review(int bookId, int userClientId, string content, int stars, string username)
        {
            BookId = bookId;
            UserClientId = userClientId;
            Content = content;
            Stars = stars;
            Username = username; 
        }
        
        /// <summary>
        /// Actualiza el contenido y la calificación de la reseña.
        /// </summary>
        /// <param name="newContent">El nuevo contenido textual de la reseña.</param>
        /// <param name="newStars">La nueva calificación por estrellas (1 a 5).</param>
        public void Update(string newContent, int newStars)
        {
            if (string.IsNullOrWhiteSpace(newContent)) 
                throw new ArgumentException("Content cannot be empty or whitespace.", nameof(newContent));
            if (newStars < 1 || newStars > 5) 
                throw new ArgumentOutOfRangeException(nameof(newStars), "Stars must be between 1 and 5.");

            Content = newContent;
            Stars = newStars;
        }
        
        /// <summary>
        /// Anonimiza el comentario asignándolo al usuario eliminado.
        /// </summary>
        public void Anonymize()
        {
            UserClientId = UserConstants.DeletedUserId;
            Username = UserConstants.DeletedUsername;
        }
    }
}