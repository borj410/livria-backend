using LivriaBackend.communities.Domain.Model.ValueObjects;
using System;

namespace LivriaBackend.communities.Domain.Model.Aggregates
{
    /// <summary>
    /// Representa una reacción de un usuario hacia una publicación (Post).
    /// Esta entidad modela la relación NxN entre UserClient y Post.
    /// </summary>
    public class PostReaction
    {
        /// <summary>
        /// Obtiene el identificador único de la reacción.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Obtiene el identificador del usuario que realizó la reacción.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Obtiene el identificador de la publicación a la que se reacciona.
        /// </summary>
        public int PostId { get; private set; }

        /// <summary>
        /// Obtiene el tipo de reacción (Like, Dislike, etc.).
        /// </summary>
        public ReactionType Type { get; private set; }

        /// <summary>
        /// Obtiene la fecha y hora de la última actualización de la reacción.
        /// </summary>
        public DateTime UpdatedAt { get; private set; }

        /// <summary>
        /// Constructor protegido sin parámetros para uso de Entity Framework Core.
        /// </summary>
        protected PostReaction() { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PostReaction"/>.
        /// </summary>
        public PostReaction(int userId, int postId, ReactionType type)
        {
            if (type == ReactionType.None)
            {
                throw new ArgumentException("Cannot create a reaction with type None.", nameof(type));
            }

            UserId = userId;
            PostId = postId;
            Type = type;
            UpdatedAt = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Actualiza el tipo de reacción.
        /// </summary>
        /// <param name="newType">El nuevo tipo de reacción.</param>
        public void UpdateReactionType(ReactionType newType)
        {
            if (newType == ReactionType.None)
            {
                 throw new ArgumentException("Use deletion logic to remove a reaction, not the Update method.", nameof(newType));
            }
            if (Type != newType)
            {
                Type = newType;
                UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}