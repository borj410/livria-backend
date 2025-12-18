using System.Collections.Generic;
using LivriaBackend.commerce.Domain.Model.Aggregates;

namespace LivriaBackend.commerce.Domain.Model.Entities
{
    /// <summary>
    /// Representa una entidad que contiene una lista de libros recomendados para un usuario específico.
    /// </summary>
    public class Recommendation
    {
        /// <summary>
        /// Obtiene el identificador único del cliente de usuario para quien se generan las recomendaciones.
        /// </summary>
        public int UserClientId { get; private set; }

        /// <summary>
        /// Obtiene la colección de libros recomendados.
        /// </summary>
        public IEnumerable<Book> RecommendedBooks { get; private set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Recommendation"/>.
        /// </summary>
        /// <param name="userClientId">El identificador del cliente de usuario.</param>
        /// <param name="recommendedBooks">La colección de libros recomendados. Si es nula, se inicializa como una lista vacía.</param>
        public Recommendation(int userClientId, IEnumerable<Book> recommendedBooks)
        {
            UserClientId = userClientId;
            RecommendedBooks = recommendedBooks ?? new List<Book>(); 
        }
    }
}