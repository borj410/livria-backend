using LivriaBackend.commerce.Domain.Model.Aggregates; 
using LivriaBackend.commerce.Domain.Model.Entities;    
using LivriaBackend.commerce.Domain.Model.Queries;
using LivriaBackend.commerce.Domain.Repositories; 
using LivriaBackend.commerce.Domain.Model.Services;
using LivriaBackend.users.Domain.Model.Repositories; 
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Application.Internal.QueryServices
{
    /// <summary>
    /// Implementa el servicio de consultas para la generación de recomendaciones de libros.
    /// Basado en los géneros favoritos de un usuario, sugiere libros que aún no ha añadido a favoritos.
    /// </summary>
    public class RecommendationQueryService : IRecommendationQueryService
    {
        private readonly IUserClientRepository _userClientRepository;
        private readonly IBookRepository _bookRepository;
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RecommendationQueryService"/>.
        /// </summary>
        /// <param name="userClientRepository">El repositorio de clientes de usuario para obtener datos del usuario.</param>
        /// <param name="bookRepository">El repositorio de libros para acceder a todos los libros disponibles.</param>

        public RecommendationQueryService(IUserClientRepository userClientRepository, IBookRepository bookRepository)
        {
            _userClientRepository = userClientRepository;
            _bookRepository = bookRepository;
        }
        /// <summary>
        /// Maneja el comando <see cref="GetUserRecommendationsQuery"/> para generar recomendaciones de libros para un usuario.
        /// Las recomendaciones se basan en los géneros de los libros favoritos del usuario.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID del cliente de usuario.</param>
        /// <returns>Un objeto <see cref="Recommendation"/> que contiene el ID del usuario y una lista de libros recomendados.</returns>
        /// <exception cref="ArgumentException">Se lanza si el cliente de usuario no se encuentra.</exception>
        /// <remarks>
        /// Este método:
        /// 1. Busca el <see cref="UserClient"/> por su ID.
        /// 2. Extrae los géneros favoritos del usuario de sus libros favoritos.
        /// 3. Si no hay géneros favoritos, devuelve una recomendación vacía.
        /// 4. Recupera todos los libros disponibles.
        /// 5. Filtra los libros para recomendar aquellos que coincidan con los géneros favoritos del usuario
        ///    y que el usuario aún no tenga en su lista de favoritos.
        /// </remarks>
        public async Task<Recommendation> Handle(GetUserRecommendationsQuery query)
        {
            
            var userClient = await _userClientRepository.GetByIdAsync(query.UserClientId);

            if (userClient == null)
            {
                throw new ArgumentException($"UserClient with ID {query.UserClientId} not found.");
            }

            
            var favoriteGenres = userClient.FavoriteBooks
                                           .Select(b => b.Genre)
                                           .Where(g => !string.IsNullOrWhiteSpace(g))
                                           .Distinct()
                                           .ToList();

            if (!favoriteGenres.Any())
            {
               
                return new Recommendation(query.UserClientId, new List<Book>());
            }

           
            var allBooks = await _bookRepository.GetAllAsync(); 

            var recommendedBooks = allBooks
                .Where(book => favoriteGenres.Contains(book.Genre) && !userClient.FavoriteBooks.Any(fb => fb.Id == book.Id))
                .ToList();

            return new Recommendation(query.UserClientId, recommendedBooks);
        }
    }
}