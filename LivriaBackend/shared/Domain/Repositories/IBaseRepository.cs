using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using LivriaBackend.shared.Domain.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;

namespace LivriaBackend.shared.Infrastructure.Persistence.EFC.Repositories
{
    /// <summary>
    /// Clase abstracta base para implementaciones de repositorios utilizando Entity Framework Core.
    /// Proporciona una implementación genérica de las operaciones definidas en <see cref="IAsyncRepository{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad con la que trabaja este repositorio.</typeparam>
    public abstract class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// El contexto de la base de datos de la aplicación, utilizado para interactuar con la base de datos.
        /// </summary>
        protected readonly AppDbContext Context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="BaseRepository{TEntity}"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación (<see cref="AppDbContext"/>).</param>
        protected BaseRepository(AppDbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Obtiene una entidad por su identificador único de forma asíncrona.
        /// Este método es virtual para permitir a las clases derivadas anularlo si necesitan
        /// lógica de carga específica (ej. incluir relaciones).
        /// </summary>
        /// <param name="id">El identificador único de la entidad.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es la entidad encontrada, o <c>null</c> si no existe.
        /// </returns>
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Obtiene una colección de todas las entidades de forma asíncrona.
        /// Este método es virtual para permitir a las clases derivadas anularlo si necesitan
        /// lógica de carga específica (ej. incluir relaciones).
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es una colección de todas las entidades.
        /// Retorna una colección vacía si no hay entidades.
        /// </returns>
        public virtual async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// Añade una nueva entidad al contexto de la base de datos de forma asíncrona.
        /// Los cambios se persistirán en la base de datos cuando se llame a <c>IUnitOfWork.CompleteAsync()</c>.
        /// </summary>
        /// <param name="entity">La instancia de la entidad a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        /// <summary>
        /// Elimina una entidad del contexto de la base de datos.
        /// Los cambios se persistirán en la base de datos cuando se llame a <c>IUnitOfWork.CompleteAsync()</c>.
        /// </summary>
        /// <param name="entity">La instancia de la entidad a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public Task DeleteAsync(TEntity entity) 
        {
            Context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask; 
        }
        
        public Task UpdateAsync(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Verifica de forma asíncrona si una entidad con el identificador especificado existe.
        /// Asume que la entidad tiene una propiedad "Id" de tipo entero.
        /// </summary>
        /// <param name="id">El identificador único de la entidad a verificar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es <c>true</c> si la entidad existe; de lo contrario, <c>false</c>.
        /// </returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await Context.Set<TEntity>().AnyAsync(e => EF.Property<int>(e, "Id") == id);
        }
    }
}