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
    /// Implementación concreta del repositorio de <see cref="CartItem"/> utilizando Entity Framework Core.
    /// Hereda de <see cref="BaseRepository{TEntity}"/> y <see cref="ICartItemRepository"/>.
    /// </summary>
    public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CartItemRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación.</param>
        public CartItemRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene un ítem del carrito de forma asíncrona por su identificador único, incluyendo el libro y el cliente de usuario asociados.
        /// Sobrescribe el método base para incluir la carga ansiosa de las relaciones <see cref="CartItem.Book"/> y <see cref="CartItem.UserClient"/>.
        /// </summary>
        /// <param name="id">El identificador único del ítem del carrito.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es el <see cref="CartItem"/> encontrado, o null si no existe.</returns>
        public new async Task<CartItem> GetByIdAsync(int id)
        {
            return await this.Context.CartItems
                .Include(ci => ci.Book)
                .Include(ci => ci.UserClient)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        /// <summary>
        /// Obtiene todos los ítems del carrito de un usuario específico de forma asíncrona, incluyendo el libro y el cliente de usuario asociados.
        /// </summary>
        /// <param name="userClientId">El identificador del cliente de usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="CartItem"/> para el usuario especificado.</returns>
        public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int userClientId)
        {
            return await this.Context.CartItems
                .Where(ci => ci.UserClientId == userClientId)
                .Include(ci => ci.Book)
                .Include(ci => ci.UserClient)
                .ToListAsync();
        }

        /// <summary>
        /// Añade un nuevo ítem del carrito de forma asíncrona al repositorio.
        /// </summary>
        /// <param name="cartItem">El objeto <see cref="CartItem"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddAsync(CartItem cartItem)
        {
            await this.Context.CartItems.AddAsync(cartItem);
        }

        /// <summary>
        /// Actualiza un ítem del carrito existente de forma asíncrona en el repositorio.
        /// </summary>
        /// <param name="cartItem">El objeto <see cref="CartItem"/> a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método establece el estado de la entidad a <see cref="EntityState.Modified"/>,
        /// lo que indica a Entity Framework Core que la entidad ha sido modificada y debe ser guardada.
        /// La persistencia se realiza con <c>UnitOfWork</c>.
        /// </remarks>
        public async Task UpdateAsync(CartItem cartItem)
        {
            this.Context.Entry(cartItem).State = EntityState.Modified;
            await Task.CompletedTask; 
        }

        /// <summary>
        /// Elimina un ítem del carrito existente de forma asíncrona del repositorio.
        /// </summary>
        /// <param name="cartItem">El objeto <see cref="CartItem"/> a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método marca la entidad para eliminación en el contexto.
        /// La eliminación real en la base de datos se realiza con <c>UnitOfWork</c>.
        /// </remarks>
        public async Task DeleteAsync(CartItem cartItem)
        {
            this.Context.CartItems.Remove(cartItem);
            await Task.CompletedTask; 
        }

        /// <summary>
        /// Encuentra un ítem del carrito de forma asíncrona por el ID del libro y el ID del cliente de usuario.
        /// </summary>
        /// <param name="bookId">El identificador del libro.</param>
        /// <param name="userClientId">El identificador del cliente de usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es el <see cref="CartItem"/> encontrado, o null si no existe.</returns>
        public async Task<CartItem> FindByBookAndUserAsync(int bookId, int userClientId)
        {
            return await this.Context.CartItems
                .Where(ci => ci.BookId == bookId && ci.UserClientId == userClientId)
                .FirstOrDefaultAsync();
        }
    }
}