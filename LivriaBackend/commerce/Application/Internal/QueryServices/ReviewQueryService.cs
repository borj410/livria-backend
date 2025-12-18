using LivriaBackend.commerce.Domain.Model.Entities;
using LivriaBackend.commerce.Domain.Model.Queries;
using LivriaBackend.commerce.Domain.Repositories;
using LivriaBackend.commerce.Domain.Model.Services;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LivriaBackend.commerce.Application.Internal.QueryServices
{
    /// <summary>
    /// Implementa el servicio de consultas para la entidad <see cref="Review"/>.
    /// Se encarga de procesar las solicitudes de lectura de datos de reseñas.
    /// </summary>
    public class ReviewQueryService : IReviewQueryService
    {
        private readonly IReviewRepository _reviewRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReviewQueryService"/>.
        /// </summary>
        /// <param name="reviewRepository">El repositorio de reseñas para acceder a los datos.</param>
        public ReviewQueryService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        /// <summary>
        /// Maneja el comando <see cref="GetReviewByIdQuery"/> para obtener una reseña específica por su identificador.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID de la reseña a buscar.</param>
        /// <returns>El objeto <see cref="Review"/> si se encuentra; de lo contrario, null.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="IReviewRepository"/>.
        /// </remarks>
        public async Task<Review> Handle(GetReviewByIdQuery query)
        {
            return await _reviewRepository.GetByIdAsync(query.ReviewId);
        }

        /// <summary>
        /// Maneja el comando <see cref="GetAllReviewsQuery"/> para obtener todas las reseñas disponibles.
        /// </summary>
        /// <param name="query">La consulta para obtener todas las reseñas (normalmente un objeto vacío).</param>
        /// <returns>Una colección de todos los objetos <see cref="Review"/>.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="IReviewRepository"/>.
        /// </remarks>
        public async Task<IEnumerable<Review>> Handle(GetAllReviewsQuery query)
        {
            return await _reviewRepository.GetAllAsync();
        }
        
        /// <summary>
        /// Maneja el comando <see cref="GetReviewsByBookIdQuery"/> para obtener todas las reseñas asociadas a un libro específico.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID del libro.</param>
        /// <returns>Una colección de objetos <see cref="Review"/> asociados al libro.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="IReviewRepository"/>.
        /// </remarks>
        public async Task<IEnumerable<Review>> Handle(GetReviewsByBookIdQuery query)
        {
            return await _reviewRepository.GetByBookIdAsync(query.BookId);
        }
        
        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userClientId)
        {
            return await _reviewRepository.GetReviewsByUserIdAsync(userClientId);
        }
    }
}