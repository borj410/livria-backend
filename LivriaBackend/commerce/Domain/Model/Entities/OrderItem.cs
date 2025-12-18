using LivriaBackend.commerce.Domain.Model.Aggregates; 
using System; // Necesario para ArgumentNullException, ArgumentOutOfRangeException

namespace LivriaBackend.commerce.Domain.Model.Entities
{
    /// <summary>
    /// Representa un ítem individual dentro de una orden de compra. Es una entidad de dominio.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Obtiene el identificador único del ítem de la orden.
        /// </summary>
        public int Id { get; private set; } 

        /// <summary>
        /// Obtiene el identificador único del libro original del cual se creó este ítem de la orden.
        /// </summary>
        public int BookId { get; private set; } 
       
        /// <summary>
        /// Obtiene el título del libro en el momento de la compra.
        /// </summary>
        public string BookTitle { get; private set; }

        /// <summary>
        /// Obtiene el autor del libro en el momento de la compra.
        /// </summary>
        public string BookAuthor { get; private set; }

        /// <summary>
        /// Obtiene el precio unitario del libro en el momento de la compra.
        /// </summary>
        public decimal BookPrice { get; private set; } 

        /// <summary>
        /// Obtiene la URL o ruta de la imagen de la portada del libro en el momento de la compra.
        /// </summary>
        public string BookCover { get; private set; } 

        /// <summary>
        /// Obtiene la cantidad de este libro incluida en la orden.
        /// </summary>
        public int Quantity { get; private set; } 

        /// <summary>
        /// Obtiene el total monetario de este ítem (Cantidad * Precio del libro).
        /// </summary>
        public decimal ItemTotal { get; private set; } 

        /// <summary>
        /// Obtiene el identificador único de la orden a la que pertenece este ítem.
        /// </summary>
        public int OrderId { get; private set; } 

        /// <summary>
        /// Obtiene el objeto <see cref="Order"/> al que pertenece este ítem.
        /// </summary>
        public Order Order { get; private set; } 

        /// <summary>
        /// Constructor protegido para uso de frameworks ORM (como Entity Framework Core).
        /// No debe ser utilizado directamente para la creación de instancias de <see cref="OrderItem"/>.
        /// </summary>
        protected OrderItem() { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="OrderItem"/> con los detalles del libro en el momento de la compra.
        /// </summary>
        /// <param name="bookId">El identificador del libro.</param>
        /// <param name="bookTitle">El título del libro.</param>
        /// <param name="bookAuthor">El autor del libro.</param>
        /// <param name="bookPrice">El precio unitario del libro.</param>
        /// <param name="bookCover">La URL de la portada del libro.</param>
        /// <param name="quantity">La cantidad del libro en este ítem.</param>
        /// <exception cref="ArgumentNullException">Se lanza si el título o el autor del libro son nulos o vacíos.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Se lanza si el precio o la cantidad son no positivos.</exception>
        /// <remarks>
        /// Este constructor calcula automáticamente el <see cref="ItemTotal"/> basándose en la cantidad y el precio.
        /// </remarks>
        public OrderItem(int bookId, string bookTitle, string bookAuthor, decimal bookPrice, string bookCover, int quantity)
        {
            if (string.IsNullOrWhiteSpace(bookTitle)) throw new ArgumentNullException(nameof(bookTitle), "Book title cannot be empty.");
            if (string.IsNullOrWhiteSpace(bookAuthor)) throw new ArgumentNullException(nameof(bookAuthor), "Book author cannot be empty.");
            if (bookPrice <= 0) throw new ArgumentOutOfRangeException(nameof(bookPrice), "Book price must be positive.");
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");

            BookId = bookId;
            BookTitle = bookTitle;
            BookAuthor = bookAuthor;
            BookPrice = bookPrice;
            BookCover = bookCover;
            Quantity = quantity;
            ItemTotal = quantity * bookPrice;
        }

        /// <summary>
        /// Establece la relación bidireccional con la orden a la que pertenece este ítem.
        /// Este método es de uso interno del dominio y lo llama el agregado <see cref="Order"/>.
        /// </summary>
        /// <param name="order">El objeto <see cref="Order"/> al que se asocia este ítem.</param>
        internal void SetOrder(Order order)
        {
            Order = order;
            OrderId = order.Id; 
        }
    }
}