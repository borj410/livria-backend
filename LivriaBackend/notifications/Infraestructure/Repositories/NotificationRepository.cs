using LivriaBackend.notifications.Domain.Model.Aggregates;
using LivriaBackend.notifications.Domain.Model.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration; 
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks; 

namespace LivriaBackend.notifications.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa el repositorio para la entidad agregada <see cref="Notification"/>.
    /// Proporciona métodos para la persistencia y recuperación de datos de notificaciones,
    /// interactuando directamente con la base de datos a través de Entity Framework Core.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="NotificationRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación (<see cref="AppDbContext"/>).</param>
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una notificación por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único de la notificación.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es la <see cref="Notification"/> encontrada, o <c>null</c> si no existe.
        /// </returns>
        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        }
        
        /// <summary>
        /// Obtiene una colección de todas las notificaciones (tanto activas como ocultas) para un cliente de usuario específico de forma asíncrona.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es una colección de todas las <see cref="Notification"/> asociadas al cliente de usuario.
        /// Retorna una colección vacía si no hay notificaciones para el usuario.
        /// </returns>
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userClientId)
        {
            return await _context.Notifications
                .Where(n => n.UserClientId == userClientId)
                .ToListAsync();
        }
        
        /// <summary>
        /// Obtiene una colección de notificaciones activas (no ocultas) para un cliente de usuario específico de forma asíncrona.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es una colección de <see cref="Notification"/> activas asociadas al cliente de usuario.
        /// Retorna una colección vacía si no hay notificaciones activas para el usuario.
        /// </returns>
        public async Task<IEnumerable<Notification>> GetActiveByUserIdAsync(int userClientId)
        {
            return await _context.Notifications
                .Where(n => n.UserClientId == userClientId && !n.IsHidden)
                .ToListAsync();
        }
        
        /// <summary>
        /// Añade una nueva notificación al contexto de la base de datos de forma asíncrona.
        /// Los cambios se persistirán en la base de datos cuando se llame a <c>_context.SaveChangesAsync()</c> o a <c>_unitOfWork.CompleteAsync()</c>.
        /// </summary>
        /// <param name="notification">La instancia de <see cref="Notification"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        /// <summary>
        /// Marca una notificación existente para ser actualizada en el contexto de la base de datos.
        /// Los cambios se persistirán en la base de datos cuando se llame a <c>_context.SaveChangesAsync()</c> o a <c>_unitOfWork.CompleteAsync()</c>.
        /// </summary>
        /// <param name="notification">La instancia de <see cref="Notification"/> con los datos actualizados.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task UpdateAsync(Notification notification)
        {
            
            _context.Notifications.Update(notification);
            await Task.CompletedTask; 
        }
    }
}