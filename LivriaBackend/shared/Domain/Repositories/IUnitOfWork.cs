using System.Threading.Tasks;

namespace LivriaBackend.shared.Domain.Repositories
{
    /// <summary>
    /// Define el contrato para la Unidad de Trabajo (Unit of Work).
    /// Una Unidad de Trabajo es responsable de coordinar múltiples operaciones
    /// de repositorio, asegurando que se realicen como una sola transacción atómica.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Completa la unidad de trabajo de forma asíncrona, persistiendo todos los cambios
        /// realizados dentro de esta unidad de trabajo en la base de datos.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>
        Task CompleteAsync();
    }
}