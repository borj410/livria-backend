using System.Threading.Tasks;
using LivriaBackend.users.Domain.Model.Aggregates;
using LivriaBackend.users.Domain.Model.Commands;
using LivriaBackend.users.Domain.Model.Queries;
using LivriaBackend.users.Domain.Model.Services;
using LivriaBackend.users.Interfaces.ACL;

namespace LivriaBackend.users.Application.ACL
{
    /// <summary>
    /// Fachada de contexto del cliente de usuario.
    /// Proporciona una interfaz simplificada y de desacoplamiento para que otros módulos del sistema
    /// interactúen con las funcionalidades de creación y consulta de clientes de usuario
    /// sin tener dependencias directas en los servicios de comando y consulta internos.
    /// </summary>
    public class UserClientContextFacade : IUserClientContextFacade
    {
        private readonly IUserClientCommandService _userClientCommandService;
        private readonly IUserClientQueryService _userClientQueryService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserClientContextFacade"/>.
        /// </summary>
        /// <param name="userClientCommandService">El servicio de comandos para operaciones de escritura en clientes de usuario.</param>
        /// <param name="userClientQueryService">El servicio de consulta para operaciones de lectura en clientes de usuario.</param>
        public UserClientContextFacade(
            IUserClientCommandService userClientCommandService,
            IUserClientQueryService userClientQueryService
        )
        {
            _userClientCommandService = userClientCommandService;
            _userClientQueryService = userClientQueryService;
        }

        /// <summary>
        /// Crea un nuevo cliente de usuario en el sistema.
        /// </summary>
        /// <param name="display">El nombre visible o alias del usuario.</param>
        /// <param name="username">El nombre de usuario único para el inicio de sesión.</param>
        /// <param name="email">La dirección de correo electrónico del usuario.</param>
        /// <param name="password">La contraseña del usuario.</param>
        /// <param name="icon">La URL o identificador del icono/avatar del usuario.</param>
        /// <param name="phrase">Una frase o estado personal del usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es el <see cref="UserClient.Id"/> del cliente de usuario recién creado.
        /// Retorna 0 si el cliente de usuario no pudo ser creado (ej. debido a validaciones).
        /// </returns>
        public async Task<int> CreateUserClient(string display, string username, string email, string icon, string phrase)
        {
            var createCommand = new CreateUserClientCommand(display, username, email, icon, phrase);
            var userClient = await _userClientCommandService.Handle(createCommand);
            return userClient?.Id ?? 0;
        }

        /// <summary>
        /// Verifica de forma asíncrona si un cliente de usuario con el ID especificado existe en el sistema.
        /// </summary>
        /// <param name="id">El identificador único del cliente de usuario a verificar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es <c>true</c> si el cliente de usuario existe; de lo contrario, <c>false</c>.
        /// </returns>
        public async Task<bool> ExistsUserClientById(int id)
        {
            var query = new GetUserClientByIdQuery(id);
            var userClient = await _userClientQueryService.Handle(query);
            return userClient != null; 
        }
    }
}