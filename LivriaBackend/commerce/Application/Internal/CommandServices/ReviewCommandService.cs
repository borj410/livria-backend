using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Commands;
using LivriaBackend.commerce.Domain.Model.Entities;
using LivriaBackend.commerce.Domain.Repositories;
using LivriaBackend.commerce.Domain.Model.Services;
using LivriaBackend.shared.Domain.Repositories;
using LivriaBackend.users.Domain.Model.Repositories; 
using System;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Application.Internal.CommandServices
{
    /// <summary>
    /// Implementa el servicio de comandos para la entidad <see cref="Review"/>.
    /// Procesa comandos relacionados con la creación y gestión de reseñas de libros.
    /// </summary>

    public class ReviewCommandService : IReviewCommandService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserClientRepository _userClientRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReviewCommandService"/>.
        /// </summary>
        /// <param name="reviewRepository">El repositorio de reseñas.</param>
        /// <param name="bookRepository">El repositorio de libros para verificar la existencia del libro.</param>
        /// <param name="userClientRepository">El repositorio de clientes de usuario para verificar la existencia del usuario.</param>
        /// <param name="unitOfWork">La unidad de trabajo para gestionar transacciones.</param>

        public ReviewCommandService(IReviewRepository reviewRepository, IBookRepository bookRepository, IUserClientRepository userClientRepository, IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;
            _userClientRepository = userClientRepository; 
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Maneja el comando <see cref="CreateReviewCommand"/> para crear una nueva reseña de libro.
        /// </summary>
        /// <param name="command">El comando que contiene los datos para la creación de la reseña.</param>
        /// <returns>El objeto <see cref="Review"/> creado y persistido.</returns>
        /// <exception cref="ArgumentException">
        /// Se lanza si el libro o el cliente de usuario no se encuentran.
        /// </exception>
        /// <remarks>
        /// Este método:
        /// 1. Valida la existencia del libro y del cliente de usuario asociados a la reseña.
        /// 2. Obtiene el nombre de usuario a partir del cliente de usuario para la reseña.
        /// 3. Crea una nueva instancia de <see cref="Review"/> con los datos proporcionados.
        /// 4. Añade la nueva reseña al repositorio.
        /// 5. Completa la unidad de trabajo para persistir los cambios en la base de datos.
        /// </remarks>
        public async Task<Review> Handle(CreateReviewCommand command)
        {
            var book = await _bookRepository.GetByIdAsync(command.BookId);
            if (book == null)
            {
                throw new ArgumentException($"Book with ID {command.BookId} not found for review.");
            }

            var userClient = await _userClientRepository.GetByIdAsync(command.UserClientId);
            if (userClient == null)
            {
                throw new ArgumentException($"UserClient with ID {command.UserClientId} not found. Cannot create review.");
            }

            var usernameForReview = userClient.Username; 

            var review = new Review(command.BookId, command.UserClientId, command.Content, command.Stars, usernameForReview);

            await _reviewRepository.AddAsync(review);
            await _unitOfWork.CompleteAsync(); 

            return review;
        }
        
        public async Task<Review> Handle(UpdateReviewCommand command)
        {
            var review = await _reviewRepository.GetByIdAsync(command.ReviewId);

            if (review == null)
            {
                throw new ArgumentException($"Review with ID {command.ReviewId} not found.");
            }
            
            if (review.UserClientId != command.UserClientId)
            {
                throw new UnauthorizedAccessException("Only the author can update this review.");
            }

            review.Update(command.Content, command.Stars);

            await _reviewRepository.UpdateAsync(review);
            await _unitOfWork.CompleteAsync();

            return review;
        }
        
        /// <summary>
        /// Maneja el comando de eliminación de una reseña.
        /// </summary>
        public async Task<bool> Handle(DeleteReviewCommand command)
        {
            var review = await _reviewRepository.GetByIdAsync(command.ReviewId);

            if (review == null)
            {
                return false; 
            }
            
            if (review.UserClientId != command.UserClientId)
            {
                throw new UnauthorizedAccessException("Only the author can delete this review.");
            }

            await _reviewRepository.DeleteAsync(review);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}