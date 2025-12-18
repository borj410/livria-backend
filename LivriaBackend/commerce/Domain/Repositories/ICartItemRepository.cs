using LivriaBackend.commerce.Domain.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Domain.Repositories
{
    /// <summary>
    /// Define el contrato para un repositorio de <see cref="CartItem"/>.
    /// Proporciona métodos para acceder y manipular datos de ítems del carrito.
    /// </summary>
    public interface ICartItemRepository
    {
        /// <summary>
        /// Obtiene un ítem del carrito de forma asíncrona por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del ítem del carrito.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es el <see cref="CartItem"/> encontrado, o null si no existe.</returns>
        Task<CartItem> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los ítems del carrito de un usuario específico de forma asíncrona.
        /// </summary>
        /// <param name="userClientId">El identificador del cliente de usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="CartItem"/> para el usuario especificado.</returns>
        Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(int userClientId);

        /// <summary>
        /// Añade un nuevo ítem del carrito de forma asíncrona al repositorio.
        /// </summary>
        /// <param name="cartItem">El objeto <see cref="CartItem"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task AddAsync(CartItem cartItem);

        /// <summary>
        /// Actualiza un ítem del carrito existente de forma asíncrona en el repositorio.
        /// </summary>
        /// <param name="cartItem">El objeto <see cref="CartItem"/> a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task UpdateAsync(CartItem cartItem);

        /// <summary>
        /// Elimina un ítem del carrito existente de forma asíncrona del repositorio.
        /// </summary>
        /// <param name="cartItem">El objeto <see cref="CartItem"/> a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task DeleteAsync(CartItem cartItem);

        /// <summary>
        /// Encuentra un ítem del carrito de forma asíncrona por el ID del libro y el ID del cliente de usuario.
        /// Esto es útil para verificar si un libro ya está en el carrito de un usuario.
        /// </summary>
        /// <param name="bookId">El identificador del libro.</param>
        /// <param name="userClientId">El identificador del cliente de usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es el <see cref="CartItem"/> encontrado, o null si no existe.</returns>
        Task<CartItem> FindByBookAndUserAsync(int bookId, int userClientId); 
    }
}