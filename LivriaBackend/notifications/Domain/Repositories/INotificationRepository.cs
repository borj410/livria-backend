using LivriaBackend.notifications.Domain.Model.Aggregates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.notifications.Domain.Model.Repositories
{
    /// <summary>
    /// Define las operaciones del repositorio para el agregado <see cref="Notification"/>.
    /// Proporciona un contrato para la persistencia y recuperación de datos de notificaciones.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Obtiene una notificación por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único de la notificación.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es la <see cref="Notification"/> encontrada, o <c>null</c> si no existe.
        /// </returns>
        Task<Notification> GetByIdAsync(int id);

        /// <summary>
        /// Añade una nueva notificación al repositorio de forma asíncrona.
        /// </summary>
        /// <param name="notification">La instancia de <see cref="Notification"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task AddAsync(Notification notification);

        /// <summary>
        /// Actualiza una notificación existente en el repositorio de forma asíncrona.
        /// </summary>
        /// <param name="notification">La instancia de <see cref="Notification"/> con los datos actualizados.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task UpdateAsync(Notification notification); 
        
        /// <summary>
        /// Obtiene una colección de notificaciones activas (no ocultas) para un cliente de usuario específico de forma asíncrona.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es una colección de <see cref="Notification"/> activas asociadas al cliente de usuario.
        /// Retorna una colección vacía si no hay notificaciones activas para el usuario.
        /// </returns>
        Task<IEnumerable<Notification>> GetActiveByUserIdAsync(int userClientId);

        /// <summary>
        /// Obtiene una colección de todas las notificaciones (activas y ocultas) para un cliente de usuario específico de forma asíncrona.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es una colección de todas las <see cref="Notification"/> asociadas al cliente de usuario.
        /// Retorna una colección vacía si no hay notificaciones para el usuario.
        /// </returns>
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userClientId); 
    }
}