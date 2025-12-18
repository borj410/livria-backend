using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using System.Threading.Tasks;

namespace LivriaBackend.communities.Domain.Services
{
    /// <summary>
    /// Define el contrato para el servicio de comandos de las relaciones usuario-comunidad.
    /// </summary>
    public interface IUserCommunityCommandService
    {
        /// <summary>
        /// Maneja el comando para que un usuario se una a una comunidad.
        /// </summary>
        /// <param name="command">El comando que contiene los IDs del usuario y la comunidad para la unión.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es el objeto <see cref="UserCommunity"/> creado.</returns>
        Task<UserCommunity> Handle(JoinCommunityCommand command);

        /// <summary>
        /// Maneja el comando para que un usuario salga de una comunidad.
        /// </summary>
        /// <param name="command">El comando que contiene los IDs del usuario y la comunidad para la salida.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es <c>true</c> si la membresía fue eliminada con éxito, <c>false</c> si no se encontró la membresía.</returns>
        Task<bool> Handle(LeaveCommunityCommand command); 
    }
}