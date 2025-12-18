namespace LivriaBackend.communities.Domain.Model.Commands
{
    /// <summary>
    /// Representa un comando para crear una nueva publicación (post) dentro de una comunidad específica.
    /// Este comando contiene la información que un usuario desea publicar.
    /// </summary>
    /// <param name="CommunityId">El identificador único de la comunidad donde se publicará el post.</param>
    /// <param name="Username">El nombre de usuario del cliente que está creando la publicación.</param>
    /// <param name="Content">El contenido textual de la publicación.</param>
    /// <param name="Img">La URL o ruta a una imagen opcional que acompaña a la publicación.</param>
    public record CreatePostCommand(
        int CommunityId,
        int UserId, 
        string Content,
        string Img
    );
}