using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using LivriaBackend.communities.Domain.Services;
using LivriaBackend.communities.Domain.Repositories;
using LivriaBackend.communities.Domain.Model.ValueObjects;
using LivriaBackend.shared.Domain.Repositories;
using System.Threading.Tasks;
using System;

namespace LivriaBackend.communities.Application.Internal.CommandServices
{
    public class PostReactionCommandService : IPostReactionCommandService
    {
        private readonly IPostReactionRepository _postReactionRepository;
        private readonly IPostRepository _postRepository; // Necesario para validar que el Post existe
        private readonly IUnitOfWork _unitOfWork;

        public PostReactionCommandService(
            IPostReactionRepository postReactionRepository,
            IPostRepository postRepository,
            IUnitOfWork unitOfWork)
        {
            _postReactionRepository = postReactionRepository;
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Maneja el comando de reacción a un post, realizando un UPSERT (Crear/Actualizar/Eliminar).
        /// </summary>
        /// <returns>La reacción actualizada o creada; null si la reacción fue eliminada.</returns>
        /// <exception cref="ArgumentException">Se lanza si el PostId no existe.</exception>
        public async Task<PostReaction?> Handle(PostReactionCommand command)
        {
            // 1. Validación de existencia del Post
            bool postExists = await _postRepository.ExistsAsync(command.PostId);
            if (!postExists)
            {
                throw new ArgumentException($"Post with ID {command.PostId} not found. Cannot react.");
            }
            
            // 2. Buscar si ya existe una reacción del usuario para este post
            var existingReaction = await _postReactionRepository.GetByUserIdAndPostIdAsync(command.UserId, command.PostId);
            
            if (existingReaction != null)
            {
                // -- CASO B: ELIMINAR O ACTUALIZAR --
                if (command.Type == ReactionType.None || command.Type == existingReaction.Type)
                {
                    // a) Eliminar (Si el tipo es None O si el usuario intenta reaccionar con el mismo tipo)
                    await _postReactionRepository.DeleteAsync(existingReaction);
                    await _unitOfWork.CompleteAsync();
                    return null; // Retorna null para indicar que la reacción fue eliminada
                }
                else
                {
                    // b) Actualizar (Cambio de Like a Dislike, o viceversa)
                    existingReaction.UpdateReactionType(command.Type);
                    await _postReactionRepository.UpdateAsync(existingReaction);
                    await _unitOfWork.CompleteAsync();
                    return existingReaction;
                }
            }
            else
            {
                // -- CASO A: CREAR --
                if (command.Type == ReactionType.None)
                {
                    // No se puede "eliminar" una reacción que no existe. Ignoramos y retornamos null.
                    return null;
                }
                
                var newReaction = new PostReaction(command.UserId, command.PostId, command.Type);
                await _postReactionRepository.AddAsync(newReaction);
                await _unitOfWork.CompleteAsync();
                return newReaction;
            }
        }
    }
}