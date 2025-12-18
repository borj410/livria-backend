using LivriaBackend.commerce.Domain.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Domain.Repositories
{
    /// <summary>
    /// Define el contrato para un repositorio de <see cref="Review"/>.
    /// Proporciona métodos para acceder y manipular datos de reseñas.
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Obtiene una reseña de forma asíncrona por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la reseña.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Review"/> encontrada, o null si no existe.</returns>
        Task<Review> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las reseñas de forma asíncrona.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de todas las <see cref="Review"/>.</returns>
        Task<IEnumerable<Review>> GetAllAsync();

        /// <summary>
        /// Añade una nueva reseña de forma asíncrona al repositorio.
        /// </summary>
        /// <param name="review">El objeto <see cref="Review"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task AddAsync(Review review); 
        
        /// <summary>
        /// Obtiene todas las reseñas de un libro específico de forma asíncrona.
        /// </summary>
        /// <param name="bookId">El identificador del libro.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="Review"/> para el libro especificado.</returns>
        Task<IEnumerable<Review>> GetByBookIdAsync(int bookId);

        /// <summary>
        /// Obtiene todas las reseñas creadas por un cliente específico.
        /// </summary>
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userClientId);
        
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
    }
}