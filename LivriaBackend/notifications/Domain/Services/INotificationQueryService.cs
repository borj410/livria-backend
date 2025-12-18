using LivriaBackend.notifications.Domain.Model.Aggregates;
using LivriaBackend.notifications.Domain.Model.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.notifications.Domain.Model.Services
{
    /// <summary>
    /// Define el contrato para el servicio de consulta de notificaciones.
    /// Este servicio es responsable de recuperar datos de notificaciones para su visualización o procesamiento.
    /// </summary>
    public interface INotificationQueryService
    {
        /// <summary>
        /// Maneja la consulta para obtener una notificación específica por su identificador.
        /// </summary>
        /// <param name="query">La consulta <see cref="GetNotificationByIdQuery"/> que contiene el ID de la notificación.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es la <see cref="Notification"/> encontrada, o <c>null</c> si no existe.
        /// </returns>
        Task<Notification> Handle(GetNotificationByIdQuery query);

        /// <summary>
        /// Maneja la consulta para obtener todas las notificaciones de un usuario específico.
        /// </summary>
        /// <param name="query">La consulta <see cref="GetAllNotificationsByUserIdQuery"/> que contiene el ID del cliente de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es una colección de <see cref="Notification"/> asociadas al usuario especificado.
        /// Retorna una colección vacía si no hay notificaciones para el usuario.
        /// </returns>
        Task<IEnumerable<Notification>> Handle(GetAllNotificationsByUserIdQuery query); 
    }
}