using LivriaBackend.commerce.Domain.Model.Entities;
using LivriaBackend.commerce.Domain.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de <see cref="Review"/> utilizando Entity Framework Core.
    /// Hereda de <see cref="BaseRepository{TEntity}"/> y <see cref="IReviewRepository"/>.
    /// </summary>
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReviewRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación.</param>
        public ReviewRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene una reseña de forma asíncrona por su identificador único, incluyendo el libro y el cliente de usuario asociados.
        /// Sobrescribe el método base para incluir la carga ansiosa de las relaciones <see cref="Review.Book"/> y <see cref="Review.UserClient"/>.
        /// </summary>
        /// <param name="id">El identificador único de la reseña.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Review"/> encontrada, o null si no existe.</returns>
        public new async Task<Review> GetByIdAsync(int id)
        {
            return await this.Context.Reviews
                .Include(r => r.Book)
                .Include(r => r.UserClient) 
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Obtiene todas las reseñas de forma asíncrona, incluyendo el libro y el cliente de usuario asociados.
        /// Sobrescribe el método base para incluir la carga ansiosa de las relaciones <see cref="Review.Book"/> y <see cref="Review.UserClient"/>.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de todas las <see cref="Review"/>.</returns>
        public new async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await this.Context.Reviews
                .Include(r => r.Book)
                .Include(r => r.UserClient) 
                .ToListAsync();
        }

        /// <summary>
        /// Añade una nueva reseña de forma asíncrona al repositorio.
        /// </summary>
        /// <param name="review">El objeto <see cref="Review"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddAsync(Review review)
        {
            await this.Context.Reviews.AddAsync(review);
        }
        
        public async Task UpdateAsync(Review review)
        {
            Context.Set<Review>().Update(review);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Obtiene todas las reseñas de un libro específico de forma asíncrona, incluyendo el libro y el cliente de usuario asociados.
        /// </summary>
        /// <param name="bookId">El identificador del libro.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="Review"/> para el libro especificado.</returns>
        public async Task<IEnumerable<Review>> GetByBookIdAsync(int bookId)
        {
            return await this.Context.Reviews
                .Include(r => r.Book) 
                .Include(r => r.UserClient) 
                .Where(r => r.BookId == bookId) 
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userClientId)
        {
            return await Context.Reviews
                .Where(r => r.UserClientId == userClientId)
                .ToListAsync();
        }
    }
}