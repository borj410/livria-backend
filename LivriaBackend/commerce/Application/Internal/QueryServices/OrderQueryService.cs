using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Queries;
using LivriaBackend.commerce.Domain.Repositories;
using LivriaBackend.commerce.Domain.Model.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Application.Internal.QueryServices
{
    /// <summary>
    /// Implementa el servicio de consultas para la entidad <see cref="Order"/>.
    /// Se encarga de procesar las solicitudes de lectura de datos de órdenes.
    /// </summary>
    public class OrderQueryService : IOrderQueryService
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="OrderQueryService"/>.
        /// </summary>
        /// <param name="orderRepository">El repositorio de órdenes para acceder a los datos.</param>
        public OrderQueryService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Maneja el comando <see cref="GetOrderByIdQuery"/> para obtener una orden específica por su identificador.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID de la orden a buscar.</param>
        /// <returns>El objeto <see cref="Order"/> si se encuentra; de lo contrario, null.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="IOrderRepository"/>.
        /// </remarks>
        public async Task<Order> Handle(GetOrderByIdQuery query)
        {
            return await _orderRepository.GetByIdAsync(query.OrderId);
        }

        /// <summary>
        /// Maneja el comando <see cref="GetOrdersByUserIdQuery"/> para obtener todas las órdenes de un usuario específico.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID del cliente de usuario.</param>
        /// <returns>Una colección de objetos <see cref="Order"/> asociados al usuario.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="IOrderRepository"/>.
        /// </remarks>
        public async Task<IEnumerable<Order>> Handle(GetOrdersByUserIdQuery query)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(query.UserClientId);
        }

        /// <summary>
        /// Maneja el comando <see cref="GetOrderByCodeQuery"/> para obtener una orden específica por su código de orden.
        /// </summary>
        /// <param name="query">La consulta que contiene el código de la orden a buscar.</param>
        /// <returns>El objeto <see cref="Order"/> si se encuentra; de lo contrario, null.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="IOrderRepository"/>.
        /// </remarks>
        public async Task<Order> Handle(GetOrderByCodeQuery query)
        {
            return await _orderRepository.GetOrderByCodeAsync(query.OrderCode);
        }

        /// <summary>
        /// Maneja el comando <see cref="GetAllOrdersQuery"/> para obtener todas las órdenes disponibles en el sistema.
        /// </summary>
        /// <param name="query">La consulta para obtener todas las órdenes.</param>
        /// <returns>Una colección de objetos <see cref="Order"/>.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="IOrderRepository"/>.
        /// </remarks>
        public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query) 
        {
            return await _orderRepository.FindAllAsync(); 
        }
    }
}