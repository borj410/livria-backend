using System.Collections.Generic;
using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Aggregates;
using LivriaBackend.users.Domain.Model.Queries;

namespace LivriaBackend.users.Domain.Model.Services
{
    /// <summary>
    /// Define el contrato para el servicio de consulta de clientes de usuario.
    /// Este servicio es responsable de recuperar datos de clientes de usuario para su visualización o procesamiento.
    /// </summary>
    public interface IUserClientQueryService
    {
        /// <summary>
        /// Maneja la consulta para obtener todos los clientes de usuario en el sistema.
        /// </summary>
        /// <param name="query">La consulta <see cref="GetAllUserClientQuery"/>.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es una colección de todos los objetos <see cref="UserClient"/>.
        /// Retorna una colección vacía si no hay clientes de usuario.
        /// </returns>
        Task<IEnumerable<UserClient>> Handle(GetAllUserClientQuery query);

        /// <summary>
        /// Maneja la consulta para obtener un cliente de usuario específico por su identificador único.
        /// </summary>
        /// <param name="query">La consulta <see cref="GetUserClientByIdQuery"/> que contiene el ID del cliente de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es el objeto <see cref="UserClient"/> encontrado, o <c>null</c> si no existe.
        /// </returns>
        Task<UserClient> Handle(GetUserClientByIdQuery query);
        
        Task<bool> HasCommunityPlanAsync(int userClientId);
    }
}