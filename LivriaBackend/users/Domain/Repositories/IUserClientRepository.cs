using System.Collections.Generic;
using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Aggregates;

namespace LivriaBackend.users.Domain.Model.Repositories
{
    /// <summary>
    /// Define las operaciones del repositorio para el agregado <see cref="UserClient"/>.
    /// Proporciona un contrato para la persistencia y recuperación de datos de clientes de usuario.
    /// </summary>
    public interface IUserClientRepository
    {
        /// <summary>
        /// Obtiene un cliente de usuario por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es el <see cref="UserClient"/> encontrado, o <c>null</c> si no existe.
        /// </returns>
        Task<UserClient> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene una colección de todos los clientes de usuario de forma asíncrona.
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es una colección de todos los <see cref="UserClient"/>.
        /// Retorna una colección vacía si no hay clientes de usuario.
        /// </returns>
        Task<IEnumerable<UserClient>> GetAllAsync();

        /// <summary>
        /// Obtiene un cliente de usuario por su nombre de usuario de forma asíncrona.
        /// </summary>
        /// <param name="username">El nombre de usuario del cliente a buscar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es el <see cref="UserClient"/> encontrado, o <c>null</c> si no existe.
        /// </returns>
        Task<UserClient> GetByUsernameAsync(string username); 

        /// <summary>
        /// Añade un nuevo cliente de usuario al repositorio de forma asíncrona.
        /// </summary>
        /// <param name="userClient">La instancia de <see cref="UserClient"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task AddAsync(UserClient userClient);

        /// <summary>
        /// Actualiza un cliente de usuario existente en el repositorio de forma asíncrona.
        /// </summary>
        /// <param name="userClient">La instancia de <see cref="UserClient"/> con los datos actualizados.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task UpdateAsync(UserClient userClient);

        /// <summary>
        /// Elimina un cliente de usuario del repositorio de forma asíncrona.
        /// </summary>
        /// <param name="userClient">La instancia de <see cref="UserClient"/> a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task DeleteAsync(UserClient userClient);

        /// <summary>
        /// Obtiene un cliente de usuario por su dirección de correo electrónico de forma asíncrona.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico del cliente a buscar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es el <see cref="UserClient"/> encontrado, o <c>null</c> si no existe.
        /// </returns>
        Task<UserClient> GetByEmailAsync(string email);

        /// <summary>
        /// Verifica de forma asíncrona si un cliente de usuario con el nombre de usuario especificado ya existe.
        /// </summary>
        /// <param name="username">El nombre de usuario a verificar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es <c>true</c> si el nombre de usuario ya existe; de lo contrario, <c>false</c>.
        /// </returns>
        Task<bool> ExistsByUsernameAsync(string username);

        /// <summary>
        /// Verifica de forma asíncrona si un cliente de usuario con la dirección de correo electrónico especificada ya existe.
        /// </summary>
        /// <param name="email">La dirección de correo electrónico a verificar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es <c>true</c> si la dirección de correo electrónico ya existe; de lo contrario, <c>false</c>.
        /// </returns>
        Task<bool> ExistsByEmailAsync(string email);
    }
}