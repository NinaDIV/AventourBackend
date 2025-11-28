using System;
using System.Threading.Tasks;

namespace Aventour.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Agrega aquí los repositorios a medida que los vayas creando
        IDestinoRepository Destinos { get; }
        
        // Ejemplos futuros:
        // IRutaRepository Rutas { get; }
        // IAgenciaRepository Agencias { get; }

        // Método para guardar todos los cambios en la base de datos de una sola vez
        Task<int> SaveChangesAsync();
    }
}