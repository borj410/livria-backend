using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivriaBackend.shared.Domain.Repositories
{
    /// <summary>
    /// Define una interfaz genérica para repositorios asíncronos.
    /// Este contrato asegura que cualquier repositorio que lo implemente
    /// proporcione métodos comunes para operaciones de acceso a datos.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad de dominio con la que trabaja el repositorio.</typeparam>
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Obtiene una entidad por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único de la entidad.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es la entidad encontrada, o <c>null</c> si no existe.
        /// </returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene una colección de todas las entidades de forma asíncrona.
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es una colección de todas las entidades.
        /// Retorna una colección vacía si no hay entidades.
        /// </returns>
        Task<IEnumerable<TEntity>> ListAsync();

        /// <summary>
        /// Añade una nueva entidad al repositorio de forma asíncrona.
        /// </summary>
        /// <param name="entity">La instancia de la entidad a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Elimina una entidad del repositorio de forma asíncrona.
        /// </summary>
        /// <param name="entity">La instancia de la entidad a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task DeleteAsync(TEntity entity); 
        
        /// <summary>
        /// Actualiza una entidad existente en el repositorio de forma asíncrona.
        /// </summary>
        /// <param name="entity">La instancia de la entidad a actualizar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Verifica de forma asíncrona si una entidad con el identificador especificado existe.
        /// </summary>
        /// <param name="id">El identificador único de la entidad a verificar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es <c>true</c> si la entidad existe; de lo contrario, <c>false</c>.
        /// </returns>
        Task<bool> ExistsAsync(int id);
    }
}