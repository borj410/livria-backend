using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Commands;
using LivriaBackend.commerce.Domain.Model.Entities;
using LivriaBackend.commerce.Domain.Repositories;
using LivriaBackend.commerce.Domain.Model.Services;
using LivriaBackend.shared.Domain.Repositories;
using LivriaBackend.users.Domain.Model.Repositories;
using System;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Application.Internal.CommandServices
{
    /// <summary>
    /// Implementa el servicio de comandos para la entidad <see cref="CartItem"/>.
    /// Procesa comandos relacionados con la gestión de ítems en el carrito de compras.
    /// </summary>
    public class CartItemCommandService : ICartItemCommandService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IBookRepository _bookRepository; 
        private readonly IUserClientRepository _userClientRepository; 
        private readonly IUnitOfWork _unitOfWork;

        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CartItemCommandService"/>.
        /// </summary>
        /// <param name="cartItemRepository">El repositorio de ítems del carrito.</param>
        /// <param name="bookRepository">El repositorio de libros para verificar la existencia de libros.</param>
        /// <param name="userClientRepository">El repositorio de clientes de usuario para verificar la existencia de usuarios.</param>
        /// <param name="unitOfWork">La unidad de trabajo para gestionar transacciones.</param>

        public CartItemCommandService(ICartItemRepository cartItemRepository, IBookRepository bookRepository, IUserClientRepository userClientRepository, IUnitOfWork unitOfWork)
        {
            _cartItemRepository = cartItemRepository;
            _bookRepository = bookRepository;
            _userClientRepository = userClientRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Maneja el comando <see cref="CreateCartItemCommand"/> para añadir un libro al carrito de un usuario.
        /// Si el ítem ya existe en el carrito del usuario, actualiza su cantidad; de lo contrario, crea uno nuevo.
        /// </summary>
        /// <param name="command">El comando que contiene los datos del ítem del carrito.</param>
        /// <returns>El objeto <see cref="CartItem"/> creado o actualizado.</returns>
        /// <exception cref="ArgumentException">Se lanza si el libro o el cliente de usuario no se encuentran.</exception>
        /// <remarks>
        /// Este método:
        /// 1. Verifica la existencia del libro y del cliente de usuario.
        /// 2. Busca si ya existe un <see cref="CartItem"/> para el mismo libro y usuario.
        /// 3. Si existe, actualiza la cantidad del ítem existente.
        /// 4. Si no existe, crea un nuevo <see cref="CartItem"/>.
        /// 5. Persiste los cambios a través del repositorio y la unidad de trabajo.
        /// </remarks>

        public async Task<CartItem> Handle(CreateCartItemCommand command)
        {
            var book = await _bookRepository.GetByIdAsync(command.BookId);
            if (book == null)
            {
                throw new ArgumentException($"Book with ID {command.BookId} not found.");
            }

            var userClient = await _userClientRepository.GetByIdAsync(command.UserClientId);
            if (userClient == null)
            {
                throw new ArgumentException($"UserClient with ID {command.UserClientId} not found.");
            }

            var existingCartItem = await _cartItemRepository.FindByBookAndUserAsync(command.BookId, command.UserClientId);

            if (existingCartItem != null)
            {
                existingCartItem.UpdateQuantity(existingCartItem.Quantity + command.Quantity);
                await _cartItemRepository.UpdateAsync(existingCartItem);
                await _unitOfWork.CompleteAsync();
                return existingCartItem;
            }
            else
            {
                var cartItem = new CartItem(command.BookId, command.Quantity, command.UserClientId);
                await _cartItemRepository.AddAsync(cartItem);
                await _unitOfWork.CompleteAsync();
                return cartItem;
            }
        }

        /// <summary>
        /// Maneja el comando <see cref="UpdateCartItemQuantityCommand"/> para modificar la cantidad de un ítem en el carrito.
        /// </summary>
        /// <param name="command">El comando que contiene el ID del ítem del carrito, la nueva cantidad y el ID del cliente de usuario.</param>
        /// <returns>El objeto <see cref="CartItem"/> actualizado.</returns>
        /// <exception cref="ArgumentException">Se lanza si el ítem del carrito no se encuentra o no pertenece al cliente de usuario especificado.</exception>
        /// <remarks>
        /// Este método:
        /// 1. Busca el <see cref="CartItem"/> por su ID y verifica que pertenezca al usuario.
        /// 2. Si la nueva cantidad es cero, elimina el ítem del carrito.
        /// 3. Si la nueva cantidad es mayor que cero, actualiza la cantidad del ítem.
        /// 4. Persiste los cambios a través del repositorio y la unidad de trabajo.
        /// </remarks>
        public async Task<CartItem> Handle(UpdateCartItemQuantityCommand command)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(command.CartItemId);

            if (cartItem == null || cartItem.UserClientId != command.UserClientId)
            {
                throw new ArgumentException($"CartItem with ID {command.CartItemId} not found or does not belong to UserClient {command.UserClientId}.");
            }

            if (command.NewQuantity == 0)
            {
                await _cartItemRepository.DeleteAsync(cartItem);
            }
            else
            {
                cartItem.UpdateQuantity(command.NewQuantity);
                await _cartItemRepository.UpdateAsync(cartItem);
            }

            await _unitOfWork.CompleteAsync();
            return cartItem;
        }

        /// <summary>
        /// Maneja el comando <see cref="RemoveCartItemCommand"/> para eliminar un ítem específico del carrito de un usuario.
        /// </summary>
        /// <param name="command">El comando que contiene el ID del ítem del carrito y el ID del cliente de usuario.</param>
        /// <returns>True si el ítem fue eliminado exitosamente; de lo contrario, false.</returns>
        /// <remarks>
        /// Este método:
        /// 1. Busca el <see cref="CartItem"/> por su ID y verifica que pertenezca al usuario.
        /// 2. Si se encuentra y pertenece al usuario, lo elimina del repositorio.
        /// 3. Completa la unidad de trabajo para persistir los cambios.
        /// </remarks>
        public async Task<bool> Handle(RemoveCartItemCommand command)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(command.CartItemId);

            if (cartItem == null || cartItem.UserClientId != command.UserClientId)
            {
                return false; 
            }

            await _cartItemRepository.DeleteAsync(cartItem);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}