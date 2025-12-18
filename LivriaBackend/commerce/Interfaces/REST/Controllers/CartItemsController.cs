using AutoMapper;
using LivriaBackend.commerce.Domain.Model.Commands;
using LivriaBackend.commerce.Domain.Model.Queries;
using LivriaBackend.commerce.Domain.Model.Services;
using LivriaBackend.commerce.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace LivriaBackend.commerce.Interfaces.REST.Controllers
{
    /// <summary>
    /// Controlador RESTful para gestionar las operaciones relacionadas con los ítems del carrito de compras.
    /// </summary>
    [Authorize(Roles = "UserClient,Admin")]
    [ApiController]
    [Route("api/v1/cart-items")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemCommandService _cartItemCommandService;
        private readonly ICartItemQueryService _cartItemQueryService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CartItemsController"/>.
        /// </summary>
        /// <param name="cartItemCommandService">El servicio de comandos de ítems del carrito.</param>
        /// <param name="cartItemQueryService">El servicio de consulta de ítems del carrito.</param>
        /// <param name="mapper">La instancia de AutoMapper para la transformación de objetos.</param>
        public CartItemsController(ICartItemCommandService cartItemCommandService, ICartItemQueryService cartItemQueryService, IMapper mapper)
        {
            _cartItemCommandService = cartItemCommandService;
            _cartItemQueryService = cartItemQueryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Crea un nuevo ítem en el carrito de compras.
        /// </summary>
        /// <param name="resource">Los datos del nuevo ítem del carrito a crear.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="CartItemResource"/> del ítem creado
        /// con un código 201 CreatedAtAction si la operación es exitosa.
        /// Retorna BadRequest (400) si hay un error de argumento.
        /// </returns>
        [HttpPost]
        [SwaggerOperation(
            Summary= "Crear un nuevo ítem en el carrito.",
            Description= "Crea un nuevo ítem en el carrito de un usuario en el sistema."
        )]
        public async Task<ActionResult<CartItemResource>> CreateCartItem([FromBody] CreateCartItemResource resource)
        {
            var command = _mapper.Map<CreateCartItemCommand>(resource);
            try
            {
                var cartItem = await _cartItemCommandService.Handle(command);
                var cartItemResource = _mapper.Map<CartItemResource>(cartItem);
                return CreatedAtAction(nameof(GetCartItemById), new { id = cartItem.Id }, cartItemResource);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza la cantidad de un libro en un ítem de carrito existente.
        /// </summary>
        /// <param name="id">El identificador único del ítem del carrito a actualizar.</param>
        /// <param name="userClientId">El identificador único del cliente de usuario propietario del carrito.</param>
        /// <param name="resource">El recurso que contiene la nueva cantidad deseada.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene el <see cref="CartItemResource"/> actualizado (código 200 OK)
        /// si la operación es exitosa.
        /// Retorna NotFound (404) si el ítem del carrito no existe.
        /// Retorna BadRequest (400) si hay un error de argumento.
        /// </returns>
        [HttpPut("{id}/users/{userClientId}")]
        [SwaggerOperation(
            Summary= "Actualizar la cantidad de libros de un ítem de carrito existente.",
            Description= "Te permite modificar la cantidad de libros de un ítem de carrito previamente creado."
        )]
        public async Task<ActionResult<CartItemResource>> UpdateCartItemQuantity(int id, int userClientId, [FromBody] UpdateCartItemQuantityResource resource)
        {
            var command = new UpdateCartItemQuantityCommand(id, resource.NewQuantity, userClientId);
            try
            {
                var cartItem = await _cartItemCommandService.Handle(command);
                if (cartItem == null)
                {
                    return NotFound(); 
                }
                var cartItemResource = _mapper.Map<CartItemResource>(cartItem);
                return Ok(cartItemResource);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un ítem específico del carrito de un cliente de usuario.
        /// </summary>
        /// <param name="id">El identificador único del ítem del carrito a eliminar.</param>
        /// <param name="userClientId">El identificador único del cliente de usuario propietario del carrito.</param>
        /// <returns>
        /// Una acción de resultado HTTP con un código 204 NoContent si la eliminación es exitosa.
        /// Retorna NotFound (404) si el ítem del carrito no existe.
        /// Retorna BadRequest (400) si hay un error de argumento.
        /// </returns>
        [HttpDelete("{id}/users/{userClientId}")]
        [SwaggerOperation(
            Summary= "Eliminar un ítem de un carrito de un UserClient previamente creado.",
            Description= "Elimina un ítem específico del carrito de un UserClient del sistema."
        )]
        public async Task<IActionResult> RemoveCartItem(int id, int userClientId)
        {
            var command = new RemoveCartItemCommand(id, userClientId);
            try
            {
                bool removed = await _cartItemCommandService.Handle(command);
                if (!removed)
                {
                    return NotFound(); 
                }
                return NoContent(); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los datos de un ítem de carrito específico por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del ítem del carrito.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un <see cref="CartItemResource"/> si el ítem es encontrado (código 200 OK),
        /// o un resultado NotFound (código 404) si el ítem no existe.
        /// </returns>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary= "Obtener los datos de un libro perteneciente a un carrito en específico.",
            Description= "Te muestra los datos de un libro perteneciente al carrito que buscaste."
        )]
        public async Task<ActionResult<CartItemResource>> GetCartItemById(int id)
        {
            var query = new GetCartItemByIdQuery(id);
            var cartItem = await _cartItemQueryService.Handle(query);

            if (cartItem == null)
            {
                return NotFound();
            }

            var cartItemResource = _mapper.Map<CartItemResource>(cartItem);
            return Ok(cartItemResource);
        }

        /// <summary>
        /// Obtiene todos los ítems del carrito de un usuario específico.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene una colección de <see cref="CartItemResource"/>
        /// si la operación es exitosa (código 200 OK). Puede ser una colección vacía si el usuario no tiene ítems en el carrito.
        /// </returns>
        [HttpGet("users/{userClientId}")]
        [SwaggerOperation(
            Summary= "Obtener los datos del carrito del usuario especificado.",
            Description= "Te muestra los datos del carrito del usuario especificado."
        )]
        public async Task<ActionResult<IEnumerable<CartItemResource>>> GetCartItemsByUserId(int userClientId)
        {
            var query = new GetAllCartItemsByUserIdQuery(userClientId);
            var cartItems = await _cartItemQueryService.Handle(query);
            var cartItemResources = _mapper.Map<IEnumerable<CartItemResource>>(cartItems);
            return Ok(cartItemResources);
        }
    }
}