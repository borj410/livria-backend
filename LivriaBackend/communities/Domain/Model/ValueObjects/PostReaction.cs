namespace LivriaBackend.communities.Domain.Model.ValueObjects
{
    /// <summary>
    /// Define los tipos de reacciones que un usuario puede tener hacia un post.
    /// </summary>
    public enum ReactionType
    {
        None = 0, // Usado para eliminar una reacción
        Like = 1,
        Dislike = 2
    }
}