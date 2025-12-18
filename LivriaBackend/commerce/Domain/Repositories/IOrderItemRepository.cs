using LivriaBackend.commerce.Domain.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Domain.Repositories
{
    /// <summary>
    /// Define el contrato para un repositorio de <see cref="OrderItem"/>.
    /// Proporciona métodos para acceder a datos de ítems de órdenes.
    /// </summary>
    public interface IOrderItemRepository
    {
        /// <summary>
        /// Obtiene un ítem de orden de forma asíncrona por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del ítem de la orden.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es el <see cref="OrderItem"/> encontrado, o null si no existe.</returns>
        Task<OrderItem> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todos los ítems de orden de una orden específica de forma asíncrona.
        /// </summary>
        /// <param name="orderId">El identificador de la orden.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="OrderItem"/> para la orden especificada.</returns>
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);

    }
}