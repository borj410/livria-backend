namespace LivriaBackend.commerce.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para crear una nueva reseña para un libro.
    /// </summary>
    /// <param name="BookId">El identificador único del libro al que se le hace la reseña.</param>
    /// <param name="UserClientId">El identificador único del cliente de usuario que escribe la reseña.</param>
    /// <param name="Content">El contenido textual de la reseña.</param>
    /// <param name="Stars">La calificación de la reseña, típicamente en una escala de 1 a 5.</param>
    public record CreateReviewCommand(
        int BookId,
        int UserClientId,
        string Content,
        int Stars
    );
}