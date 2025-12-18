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
    /// Implementación concreta del repositorio de <see cref="OrderItem"/> utilizando Entity Framework Core.
    /// Hereda de <see cref="BaseRepository{TEntity}"/> y <see cref="IOrderItemRepository"/>.
    /// </summary>
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="OrderItemRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación.</param>
        public OrderItemRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene un ítem de orden de forma asíncrona por su identificador único.
        /// Sobrescribe el método base.
        /// </summary>
        /// <param name="id">El identificador único del ítem de la orden.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es el <see cref="OrderItem"/> encontrado, o null si no existe.</returns>
        public new async Task<OrderItem> GetByIdAsync(int id)
        {
            return await this.Context.OrderItems
                .FirstOrDefaultAsync(oi => oi.Id == id);
        }

        /// <summary>
        /// Obtiene todos los ítems de orden de una orden específica de forma asíncrona.
        /// </summary>
        /// <param name="orderId">El identificador de la orden.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una colección de <see cref="OrderItem"/> para la orden especificada.</returns>
        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await this.Context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }
    }
}