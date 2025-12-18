namespace LivriaBackend.commerce.Domain.Model.Commands
{
    public record UpdateBookCommand(
        int Id, 
        string Title, 
        string Description, 
        string Author, 
        decimal PurchasePrice, 
        string Cover, 
        string Genre, 
        string Language);
}