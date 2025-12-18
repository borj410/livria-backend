using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Domain.Model.Services
{
    /// <summary>
    /// Interfaz para el servicio de consultas de órdenes.
    /// Define los métodos para manejar diferentes tipos de consultas relacionadas con órdenes.
    /// </summary>
    public interface IOrderQueryService
    {
        /// <summary>
        /// Maneja la consulta para obtener una orden específica por su identificador.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID de la orden a buscar.</param>
        /// <returns>El objeto <see cref="Order"/> si se encuentra; de lo contrario, null.</returns>
        Task<Order> Handle(GetOrderByIdQuery query);

        /// <summary>
        /// Maneja la consulta para obtener todas las órdenes de un usuario específico.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID del cliente de usuario.</param>
        /// <returns>Una colección de objetos <see cref="Order"/> asociados al usuario.</returns>
        Task<IEnumerable<Order>> Handle(GetOrdersByUserIdQuery query);

        /// <summary>
        /// Maneja la consulta para obtener una orden específica por su código de orden.
        /// </summary>
        /// <param name="query">La consulta que contiene el código de la orden a buscar.</param>
        /// <returns>El objeto <see cref="Order"/> si se encuentra; de lo contrario, null.</returns>
        Task<Order> Handle(GetOrderByCodeQuery query);

        /// <summary>
        /// Maneja la consulta para obtener todas las órdenes disponibles en el sistema.
        /// </summary>
        /// <param name="query">La consulta para obtener todas las órdenes.</param>
        /// <returns>Una colección de objetos <see cref="Order"/>.</returns>
        Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query); 
    }
}