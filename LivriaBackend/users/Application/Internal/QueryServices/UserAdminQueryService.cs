using System.Collections.Generic;
using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Aggregates;
using LivriaBackend.users.Domain.Model.Queries;
using LivriaBackend.users.Domain.Model.Repositories;
using LivriaBackend.users.Domain.Model.Services;

namespace LivriaBackend.users.Application.Internal.QueryServices
{
    /// <summary>
    /// Implementa el servicio de consulta para las operaciones de la entidad <see cref="UserAdmin"/>.
    /// Encapsula la lógica de negocio para recuperar datos de administradores de usuario.
    /// </summary>
    public class UserAdminQueryService : IUserAdminQueryService
    {
        private readonly IUserAdminRepository _userAdminRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserAdminQueryService"/>.
        /// </summary>
        /// <param name="userAdminRepository">El repositorio para las operaciones de datos del administrador de usuario.</param>
        public UserAdminQueryService(IUserAdminRepository userAdminRepository)
        {
            _userAdminRepository = userAdminRepository;
        }

        /// <summary>
        /// Maneja la consulta para obtener todos los administradores de usuario en el sistema.
        /// </summary>
        /// <param name="query">La consulta <see cref="GetAllUserAdminQuery"/>.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es una colección de todos los objetos <see cref="UserAdmin"/>.
        /// Retorna una colección vacía si no hay administradores de usuario.
        /// </returns>
        public async Task<IEnumerable<UserAdmin>> Handle(GetAllUserAdminQuery query)
        {
            return await _userAdminRepository.GetAllAsync();
        }
    }
}