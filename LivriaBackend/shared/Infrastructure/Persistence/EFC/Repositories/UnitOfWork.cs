using LivriaBackend.shared.Domain.Repositories;
using LivriaBackend.shared.Infrastructure.Persistence.EFC.Configuration;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context">El contexto de la base de datos de la aplicación (<see cref="AppDbContext"/>)
    /// que será gestionado por esta unidad de trabajo.</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Guarda de forma asíncrona todos los cambios pendientes en el contexto de la base de datos.
    /// Este método representa la finalización de una unidad de trabajo, persistiendo
    /// todas las operaciones de inserción, actualización y eliminación.
    /// </summary>
    /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}