using AutoMapper;
using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Commands;
using LivriaBackend.commerce.Domain.Model.Entities;
using LivriaBackend.commerce.Domain.Model.ValueObjects;
using LivriaBackend.commerce.Interfaces.REST.Resources;

namespace LivriaBackend.commerce.Interfaces.REST.Transform
{
    /// <summary>
    /// Perfil de mapeo de AutoMapper para las entidades y recursos del dominio de comercio.
    /// Define cómo se transforman los objetos del dominio en recursos REST y viceversa.
    /// </summary>
    public class MappingCommerce : Profile
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="MappingCommerce"/>.
        /// En este constructor, se definen todas las reglas de mapeo entre los objetos de origen y destino.
        /// </summary>
        public MappingCommerce()
        {
            // Mapeos de Dominio a Recurso (para respuestas de API)
            // <summary>Crea un mapeo del agregado <see cref="Book"/> a <see cref="BookResource"/>.</summary>
            CreateMap<Book, BookResource>();

            // <summary>Crea un mapeo de la entidad <see cref="Review"/> a <see cref="ReviewResource"/>.</summary>
            // <remarks>Incluye un mapeo condicional para el nombre de usuario desde <see cref="Review.UserClient"/>.</remarks>
            CreateMap<Review, ReviewResource>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserClient != null ? src.UserClient.Display : "Unknown User"));

            // <summary>Crea un mapeo de la entidad <see cref="CartItem"/> a <see cref="CartItemResource"/>.</summary>
            CreateMap<CartItem, CartItemResource>();

            // <summary>Crea un mapeo del objeto de valor <see cref="Shipping"/> a <see cref="ShippingResource"/>.</summary>
            CreateMap<Shipping, ShippingResource>();

            // <summary>Crea un mapeo de la entidad <see cref="OrderItem"/> a <see cref="OrderItemResource"/>.</summary>
            CreateMap<OrderItem, OrderItemResource>();

            // <summary>Crea un mapeo del agregado <see cref="Order"/> a <see cref="OrderResource"/>.</summary>
            // <remarks>Formatea la propiedad <see cref="Order.Date"/> a un formato ISO 8601 compatible con UTC ("yyyy-MM-ddTHH:mm:ssZ").</remarks>
            CreateMap<Order, OrderResource>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-ddTHH:mm:ssZ"))); 
            
            // <summary>Crea un mapeo de la entidad <see cref="Recommendation"/> a <see cref="RecommendationResource"/>.</summary>
            CreateMap<Recommendation, RecommendationResource>();

            // Mapeos de Recurso a Comando (para solicitudes de API)
            // <summary>Crea un mapeo del recurso <see cref="CreateBookResource"/> a <see cref="CreateBookCommand"/>.</summary>
            CreateMap<CreateBookResource, CreateBookCommand>();

            // <summary>Crea un mapeo del recurso <see cref="CreateReviewResource"/> a <see cref="CreateReviewCommand"/>.</summary>
            CreateMap<CreateReviewResource, CreateReviewCommand>();

            // <summary>Crea un mapeo del recurso <see cref="CreateCartItemResource"/> a <see cref="CreateCartItemCommand"/>.</summary>
            CreateMap<CreateCartItemResource, CreateCartItemCommand>();

            // <summary>Crea un mapeo del recurso <see cref="UpdateCartItemQuantityResource"/> a <see cref="UpdateCartItemQuantityCommand"/>.</summary>
            CreateMap<UpdateCartItemQuantityResource, UpdateCartItemQuantityCommand>();

            // <summary>Crea un mapeo del recurso <see cref="CreateOrderResource"/> a <see cref="CreateOrderCommand"/>.</summary>
            CreateMap<CreateOrderResource, CreateOrderCommand>();

            
            
            // <summary>Crea un mapeo del recurso <see cref="ShippingResource"/> al objeto de valor <see cref="Shipping"/>.</summary>
            CreateMap<ShippingResource, Shipping>();
            
            CreateMap<UpdateReviewResource, UpdateReviewCommand>();
        }
    }
}