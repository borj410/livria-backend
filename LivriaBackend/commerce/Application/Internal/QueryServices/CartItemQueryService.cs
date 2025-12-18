using LivriaBackend.commerce.Domain.Model.Entities;
using LivriaBackend.commerce.Domain.Model.Queries;
using LivriaBackend.commerce.Domain.Repositories;
using LivriaBackend.commerce.Domain.Model.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Application.Internal.QueryServices
{
    /// <summary>
    /// Implementa el servicio de consultas para la entidad <see cref="CartItem"/>.
    /// Se encarga de procesar las solicitudes de lectura de datos de ítems del carrito.
    /// </summary>
    public class CartItemQueryService : ICartItemQueryService
    {
        private readonly ICartItemRepository _cartItemRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CartItemQueryService"/>.
        /// </summary>
        /// <param name="cartItemRepository">El repositorio de ítems del carrito para acceder a los datos.</param>
        public CartItemQueryService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        /// <summary>
        /// Maneja el comando <see cref="GetCartItemByIdQuery"/> para obtener un ítem del carrito específico por su identificador.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID del ítem del carrito a buscar.</param>
        /// <returns>El objeto <see cref="CartItem"/> si se encuentra; de lo contrario, null.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="ICartItemRepository"/>.
        /// </remarks>
        public async Task<CartItem> Handle(GetCartItemByIdQuery query)
        {
            return await _cartItemRepository.GetByIdAsync(query.CartItemId);
        }

        /// <summary>
        /// Maneja el comando <see cref="GetAllCartItemsByUserIdQuery"/> para obtener todos los ítems del carrito de un usuario específico.
        /// </summary>
        /// <param name="query">La consulta que contiene el ID del cliente de usuario.</param>
        /// <returns>Una colección de objetos <see cref="CartItem"/> asociados al usuario.</returns>
        /// <remarks>
        /// Este método delega la lógica de recuperación de datos al <see cref="ICartItemRepository"/>.
        /// </remarks>
        public async Task<IEnumerable<CartItem>> Handle(GetAllCartItemsByUserIdQuery query)
        {
            return await _cartItemRepository.GetCartItemsByUserIdAsync(query.UserClientId);
        }
    }
}