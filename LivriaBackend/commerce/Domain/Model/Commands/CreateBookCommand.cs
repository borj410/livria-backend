namespace LivriaBackend.commerce.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para crear un nuevo libro en el sistema.
    /// NOTA: SalePrice y PurchasePrice son generados automáticamente por la entidad Book.
    /// </summary>
    /// <param name="Title">El título del libro.</param>
    /// <param name="Description">La descripción detallada del libro.</param>
    /// <param name="Author">El autor del libro.</param>
    /// <param name="Stock">La cantidad de stock inicial del libro.</param>
    /// <param name="Cover">La URL o ruta de la imagen de la portada del libro.</param>
    /// <param name="Genre">El género del libro.</param>
    /// <param name="Language">El idioma del libro.</param>
    public record CreateBookCommand(
        string Title,
        string Description,
        string Author,
        int Stock,
        string Cover,
        string Genre,
        string Language
    );
}