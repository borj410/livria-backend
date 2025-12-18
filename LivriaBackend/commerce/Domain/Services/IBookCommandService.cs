using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Commands;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Domain.Model.Services
{
    public interface IBookCommandService
    {
        Task<Book> Handle(CreateBookCommand command);
        
        /// <summary>
        /// Maneja el comando para actualizar el stock de un libro existente añadiendo una cantidad específica.
        /// </summary>
        /// <param name="command">El comando que contiene el ID del libro y la cantidad a añadir.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es el objeto <see cref="Book"/> actualizado, o <c>null</c> si el libro no se encuentra.
        /// </returns>
        Task<Book?> Handle(UpdateBookStockCommand command); 
        
        Task<Book?> Handle(UpdateBookCommand command);
        Task<bool> Handle(DeleteBookCommand command);
        Task<Book?> Handle(ReactivateBookCommand command);
    }
}