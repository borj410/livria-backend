using System.Collections.Generic;
using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Aggregates;
using LivriaBackend.users.Domain.Model.Queries;

namespace LivriaBackend.users.Domain.Model.Services
{
    /// <summary>
    /// Define el contrato para el servicio de consulta de administradores de usuario.
    /// Este servicio es responsable de recuperar datos de administradores de usuario para su visualización o procesamiento.
    /// </summary>
    public interface IUserAdminQueryService
    {
        /// <summary>
        /// Maneja la consulta para obtener todos los administradores de usuario en el sistema.
        /// </summary>
        /// <param name="query">La consulta <see cref="GetAllUserAdminQuery"/>.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es una colección de todos los objetos <see cref="UserAdmin"/>.
        /// Retorna una colección vacía si no hay administradores de usuario.
        /// </returns>
        Task<IEnumerable<UserAdmin>> Handle(GetAllUserAdminQuery query);
    }
}