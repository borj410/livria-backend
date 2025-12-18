using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Commands;
using LivriaBackend.commerce.Domain.Repositories;
using LivriaBackend.commerce.Domain.Model.Services;
using LivriaBackend.shared.Domain.Repositories;
using System.Threading.Tasks;
using LivriaBackend.shared.Domain.Exceptions;
using LivriaBackend.users.Domain.Model.Services;
using System; 

namespace LivriaBackend.commerce.Application.Internal.CommandServices
{
    /// <summary>
    /// Implementa el servicio de comandos para la entidad <see cref="Book"/>.
    /// Procesa comandos relacionados con la gestión de libros, coordinando con el repositorio y la unidad de trabajo.
    /// </summary>
    public class BookCommandService : IBookCommandService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAdminCommandService _userAdminCommandService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BookCommandService"/>.
        /// </summary>
        /// <param name="bookRepository">El repositorio de libros para operaciones de persistencia.</param>
        /// <param name="unitOfWork">La unidad de trabajo para gestionar transacciones y guardar cambios.</param>
        /// <param name="userAdminCommandService">El servicio de comandos de UserAdmin para actualizar su capital.</param>
        public BookCommandService(IBookRepository bookRepository, IUnitOfWork unitOfWork, IUserAdminCommandService userAdminCommandService)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
            _userAdminCommandService = userAdminCommandService;
        }

        /// <summary>
        /// Maneja el comando <see cref="CreateBookCommand"/> para crear un nuevo libro.
        /// </summary>
        /// <param name="command">El comando que contiene los datos para la creación del libro.</param>
        /// <returns>El objeto <see cref="Book"/> creado y persistido.</returns>
        /// <remarks>
        /// Este método:
        /// 1. Valida que no exista un libro con el mismo título.
        /// 2. Crea una nueva instancia de <see cref="Book"/> utilizando los datos proporcionados en el comando.
        ///    NOTA: El `SalePrice` y `PurchasePrice` son generados internamente por el constructor de `Book`.
        /// 3. Añade el nuevo libro al repositorio.
        /// 4. Completa la unidad de trabajo para persistir los cambios en la base de datos.
        /// 5. **Resta el costo de compra total del stock inicial del capital del UserAdmin.**
        /// </remarks>
        public async Task<Book> Handle(CreateBookCommand command)
        {
            if (await _bookRepository.ExistsByTitleAndAuthorAsync(command.Title, command.Author))
            {
                throw new ArgumentException("A book with this title and author already exists in the catalog.");
            }

            var book = new Book(
                command.Title,
                command.Description,
                command.Author,
                command.Stock,
                command.Cover,
                command.Genre,
                command.Language
            );

            await _bookRepository.AddAsync(book);
            
            // Capital
            decimal costToSubtract = book.PurchasePrice * book.Stock;
            await _userAdminCommandService.UpdateUserAdminCapitalAsync(1, -costToSubtract);
            await _unitOfWork.CompleteAsync(); 
            return book;
        }

        /// <summary>
        /// Maneja el comando <see cref="UpdateBookStockCommand"/> para **aumentar** el stock de un libro.
        /// </summary>
        /// <param name="command">El comando que contiene el ID del libro y la cantidad a añadir al stock.</param>
        /// <returns>El objeto <see cref="Book"/> actualizado, o <c>null</c> si el libro no se encuentra.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se lanza si la cantidad a añadir es negativa.</exception>
        public async Task<Book?> Handle(UpdateBookStockCommand command)
        {
            var book = await _bookRepository.GetByIdAsync(command.BookId);
            if (book == null)
            {
                return null; 
            }

            if (!book.IsActive)
            {
                throw new InvalidOperationException("Cannot add stock to a deactivated book.");
            }
            
            if (command.QuantityToAdd <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(command.QuantityToAdd), "Quantity to add to stock must be positive.");
            }

            
            decimal costOfNewStock = book.PurchasePrice * command.QuantityToAdd;

            
            book.AddStock(command.QuantityToAdd);

            
            int userAdminToUpdateId = 1; 
            await _userAdminCommandService.UpdateUserAdminCapitalAsync(userAdminToUpdateId, -costOfNewStock);
            
            await _unitOfWork.CompleteAsync(); 
            return book;
        }

        public async Task<Book?> Handle(UpdateBookCommand command)
        {
            var book = await _bookRepository.GetByIdAsync(command.Id);
            
            if (book == null) return null;
            
            if (!book.IsActive)
            {
                throw new InvalidOperationException("Cannot update a deactivated book. Please reactivate it first.");
            }
            
            if (book.Title != command.Title || book.Author != command.Author)
            {
                if (await _bookRepository.ExistsByTitleAndAuthorAsync(command.Title, command.Author))
                {
                    throw new ArgumentException("Another active book with this title and author already exists.");
                }
            }

            book.Update(command.Title, command.Description, command.Author, 
                command.PurchasePrice, command.Cover, command.Genre, command.Language);

            await _bookRepository.UpdateAsync(book);
            await _unitOfWork.CompleteAsync();
            return book;
        }

        public async Task<bool> Handle(DeleteBookCommand command)
        {
            var book = await _bookRepository.GetByIdAsync(command.Id);
            if (book == null) return false;
            
            book.Deactivate(); 
            
            await _bookRepository.DeleteAsync(book);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        
        public async Task<Book?> Handle(ReactivateBookCommand command)
        {
            var book = await _bookRepository.GetByIdAsync(command.BookId);
            if (book == null) return null;

            if (book.IsActive) return book;
            
            if (await _bookRepository.ExistsByTitleAndAuthorAsync(book.Title, book.Author))
            {
                throw new ArgumentException("Cannot reactivate: another active book with the same title and author already exists.");
            }
            
            book.Reactivate();

            await _bookRepository.UpdateAsync(book);
            await _unitOfWork.CompleteAsync();
            return book;
        }
    }
}