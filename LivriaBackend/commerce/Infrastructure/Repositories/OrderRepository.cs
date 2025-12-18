using LivriaBackend.commerce.Domain.Model.Aggregates;
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
    /// Implementación concreta del repositorio de <see cref="Order"/> utilizando Entity Framework Core.
    /// Hereda de <see cref="BaseRepository{TEntity}"/> y <see cref="IOrderRepository"/>.
    /// </summary>
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="OrderRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación.</param>
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene una orden de forma asíncrona por su identificador único, incluyendo sus ítems, el cliente de usuario y los detalles de envío.
        /// Sobrescribe el método base para incluir la carga ansiosa de las relaciones <see cref="Order.Items"/>, <see cref="Order.UserClient"/> y <see cref="Order.Shipping"/>.
        /// </summary>
        /// <param name="id">El identificador único de la orden.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Order"/> encontrada con sus relaciones, o null si no existe.</returns>
        public new async Task<Order> GetByIdAsync(int id)
        {
            return await this.Context.Set<Order>() // Usamos Context.Set<Order>() para asegurar que se accede a la tabla de órdenes
                .Include(o => o.Items) 
                .Include(o => o.UserClient) 
                .Include(o => o.Shipping)   
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Obtiene todas las órdenes de un usuario específico de forma asíncrona, incluyendo sus ítems, el cliente de usuario y los detalles de envío.
        /// </summary>
        /// <param name="userClientId">El identificador del cliente de usuario.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="Order"/> para el usuario especificado con sus relaciones.</returns>
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userClientId)
        {
            return await this.Context.Set<Order>() 
                .Where(o => o.UserClientId == userClientId)
                .Include(o => o.Items)
                .Include(o => o.UserClient)
                .Include(o => o.Shipping)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una orden de forma asíncrona por su código de orden único, incluyendo sus ítems, el cliente de usuario y los detalles de envío.
        /// </summary>
        /// <param name="code">El código alfanumérico de la orden.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es la <see cref="Order"/> encontrada con sus relaciones, o null si no existe.</returns>
        public async Task<Order> GetOrderByCodeAsync(string code)
        {
            return await this.Context.Set<Order>() 
                .Include(o => o.Items)
                .Include(o => o.UserClient)
                .Include(o => o.Shipping)
                .FirstOrDefaultAsync(o => o.Code == code);
        }

        /// <summary>
        /// Añade una nueva orden de forma asíncrona al repositorio.
        /// </summary>
        /// <param name="order">El objeto <see cref="Order"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddAsync(Order order)
        {
            await this.Context.Set<Order>().AddAsync(order); 
        }

        /// <summary>
        /// Actualiza una orden existente de forma asíncrona en el repositorio.
        /// </summary>
        /// <param name="order">El objeto <see cref="Order"/> a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método establece el estado de la entidad a <see cref="EntityState.Modified"/>,
        /// lo que indica a Entity Framework Core que la entidad ha sido modificada y debe ser guardada.
        /// La persistencia se realiza con <c>UnitOfWork</c>.
        /// </remarks>
        public async Task UpdateAsync(Order order)
        {
            this.Context.Entry(order).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        /// <summary>
        /// Elimina una orden existente de forma asíncrona del repositorio.
        /// </summary>
        /// <param name="order">El objeto <see cref="Order"/> a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método marca la entidad para eliminación en el contexto.
        /// La eliminación real en la base de datos se realiza con <c>UnitOfWork</c>.
        /// </remarks>
        public async Task DeleteAsync(Order order)
        {
            this.Context.Set<Order>().Remove(order); 
            await Task.CompletedTask;
        }

        /// <summary>
        /// Obtiene todas las órdenes del repositorio de forma asíncrona.
        /// Incluye sus ítems, el cliente de usuario y los detalles de envío.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de todas las <see cref="Order"/> con sus relaciones.</returns>
        public async Task<IEnumerable<Order>> FindAllAsync() 
        {
            return await this.Context.Set<Order>()
                .Include(o => o.Items)
                .Include(o => o.UserClient)
                .Include(o => o.Shipping)
                .ToListAsync();
        }
    }
}