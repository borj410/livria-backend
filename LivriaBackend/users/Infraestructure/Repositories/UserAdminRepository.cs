using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Aggregates;
using LivriaBackend.users.Domain.Model.Repositories;

namespace LivriaBackend.users.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa el repositorio para la entidad agregada <see cref="UserAdmin"/>.
    /// Proporciona métodos para la persistencia y recuperación de datos de administradores de usuario,
    /// interactuando directamente con la base de datos a través de Entity Framework Core.
    /// </summary>
    public class UserAdminRepository : IUserAdminRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserAdminRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación (<see cref="AppDbContext"/>).</param>
        public UserAdminRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene un administrador de usuario por su identificador único de forma asíncrona.
        /// </summary>
        /// <param name="id">El identificador único del administrador de usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es el <see cref="UserAdmin"/> encontrado, o <c>null</c> si no existe.
        /// </returns>
        public async Task<UserAdmin> GetByIdAsync(int id)
        {
            return await _context.UserAdmins.FirstOrDefaultAsync(ua => ua.Id == id);
        }

        /// <summary>
        /// Obtiene una colección de todos los administradores de usuario de forma asíncrona.
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea es una colección de todos los <see cref="UserAdmin"/>.
        /// Retorna una colección vacía si no hay administradores de usuario.
        /// </returns>
        public async Task<IEnumerable<UserAdmin>> GetAllAsync()
        {
            return await _context.UserAdmins.ToListAsync();
        }

        /// <summary>
        /// Añade un nuevo administrador de usuario al repositorio de forma asíncrona.
        /// Los cambios se guardan inmediatamente en la base de datos.
        /// </summary>
        /// <param name="userAdmin">La instancia de <see cref="UserAdmin"/> a añadir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task AddAsync(UserAdmin userAdmin)
        {
            await _context.UserAdmins.AddAsync(userAdmin);
            await _context.SaveChangesAsync(); 
        }

        /// <summary>
        /// Actualiza un administrador de usuario existente en el repositorio de forma asíncrona.
        /// Los cambios se guardan inmediatamente en la base de datos.
        /// </summary>
        /// <param name="userAdmin">La instancia de <see cref="UserAdmin"/> con los datos actualizados.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task UpdateAsync(UserAdmin userAdmin)
        {
            _context.UserAdmins.Update(userAdmin);
            await _context.SaveChangesAsync(); 
        }

        /// <summary>
        /// Elimina un administrador de usuario del repositorio por su identificador único de forma asíncrona.
        /// Si el administrador de usuario existe, se elimina y los cambios se guardan en la base de datos.
        /// </summary>
        /// <param name="id">El identificador único del administrador de usuario a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task DeleteAsync(int id)
        {
            var userAdminToDelete = await _context.UserAdmins.FirstOrDefaultAsync(ua => ua.Id == id);
            if (userAdminToDelete != null)
            {
                _context.UserAdmins.Remove(userAdminToDelete);
                await _context.SaveChangesAsync(); 
            }
        }
    }
}