using System.Threading.Tasks;

namespace LivriaBackend.users.Interfaces.ACL
{
    /// <summary>
    /// Define la interfaz para la fachada del contexto del cliente de usuario (ACL - Anti-Corruption Layer).
    /// Esta fachada proporciona una interfaz simplificada y desacoplada para que otros módulos
    /// del sistema interactúen con las funcionalidades de creación y consulta de clientes de usuario,
    /// encapsulando la complejidad del dominio de usuarios.
    /// </summary>
    public interface IUserClientContextFacade
    {
        /// <summary>
        /// Crea un nuevo cliente de usuario en el sistema de forma asíncrona.
        /// </summary>
        /// <param name="display">El nombre visible o alias del usuario.</param>
        /// <param name="username">El nombre de usuario único para el inicio de sesión.</param>
        /// <param name="email">La dirección de correo electrónico del usuario.</param>
        /// <param name="icon">La URL o identificador del icono/avatar del usuario.</param>
        /// <param name="phrase">Una frase o estado personal del usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es el **ID del cliente de usuario** recién creado.
        /// Retorna 0 si el cliente de usuario no pudo ser creado (ej. debido a validaciones).
        /// </returns>
        Task<int> CreateUserClient(string display, string username, string email, string icon, string phrase);

        /// <summary>
        /// Verifica de forma asíncrona si un cliente de usuario con el ID especificado existe en el sistema.
        /// </summary>
        /// <param name="id">El identificador único del cliente de usuario a verificar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona.
        /// El resultado de la tarea es <c>true</c> si el cliente de usuario existe; de lo contrario, <c>false</c>.
        /// </returns>
        Task<bool> ExistsUserClientById(int id);
    }
}