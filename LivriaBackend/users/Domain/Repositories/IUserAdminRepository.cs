using System.Collections.Generic;
using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Aggregates;

namespace LivriaBackend.users.Domain.Model.Repositories
{
    /// <summary>
    /// Define las operaciones del repositorio para el agregado <see cref="UserAdmin"/>.
    /// Proporciona un contrato para la persistencia y recuperación de datos de administradores de usuario.
    /// </summary>
    public interface IUserAdminRepository
    {
        /// <summary>
        /// Obtiene un administrador de usuario por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único del administrador de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es el <see cref="UserAdmin"/> encontrado, o <c>null</c> si no existe.
        /// </returns>
        Task<UserAdmin> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene una colección de todos los administradores de usuario de forma asíncrona.
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es una colección de todos los <see cref="UserAdmin"/>.
        /// Retorna una colección vacía si no hay administradores de usuario.
        /// </returns>
        Task<IEnumerable<UserAdmin>> GetAllAsync();

        /// <summary>
        /// Añade un nuevo administrador de usuario al repositorio de forma asíncrona.
        /// </summary>
        /// <param name="userAdmin">La instancia de <see cref="UserAdmin"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task AddAsync(UserAdmin userAdmin);

        /// <summary>
        /// Actualiza un administrador de usuario existente en el repositorio de forma asíncrona.
        /// </summary>
        /// <param name="userAdmin">La instancia de <see cref="UserAdmin"/> con los datos actualizados.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task UpdateAsync(UserAdmin userAdmin); 

        /// <summary>
        /// Elimina un administrador de usuario del repositorio por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único del administrador de usuario a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task DeleteAsync(int id); 
    }
}