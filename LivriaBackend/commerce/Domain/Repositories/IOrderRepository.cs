using LivriaBackend.commerce.Domain.Model.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace LivriaBackend.commerce.Domain.Repositories
{
    /// <summary>
    /// Define el contrato para un repositorio de <see cref="Order"/>.
    /// Proporciona métodos para acceder y manipular datos de órdenes de compra.
    /// </summary>
    public interface IOrderRepository 
    {
        /// <summary>
        /// Obtiene una orden de forma asíncrona por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único de la orden.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Order"/> encontrada, o null si no existe.</returns>
        Task<Order> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene todas las órdenes de un usuario específico de forma asíncrona.
        /// </summary>
        /// <param name="userClientId">El identificador del cliente de usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="Order"/> para el usuario especificado.</returns>
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userClientId);

        /// <summary>
        /// Obtiene una orden de forma asíncrona por su código de orden único.
        /// </summary>
        /// <param name="code">El código alfanumérico de la orden.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Order"/> encontrada, o null si no existe.</returns>
        Task<Order> GetOrderByCodeAsync(string code); 

        /// <summary>
        /// Añade una nueva orden de forma asíncrona al repositorio.
        /// </summary>
        /// <param name="order">El objeto <see cref="Order"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task AddAsync(Order order);

        /// <summary>
        /// Actualiza una orden existente de forma asíncrona en el repositorio.
        /// </summary>
        /// <param name="order">El objeto <see cref="Order"/> a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task UpdateAsync(Order order);

        /// <summary>
        /// Elimina una orden existente de forma asíncrona del repositorio.
        /// </summary>
        /// <param name="order">El objeto <see cref="Order"/> a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task DeleteAsync(Order order);

        /// <summary>
        /// Obtiene todas las órdenes del repositorio de forma asíncrona.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de todas las <see cref="Order"/>.</returns>
        Task<IEnumerable<Order>> FindAllAsync(); 
    }
}