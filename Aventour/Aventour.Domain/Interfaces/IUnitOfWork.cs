using System;
 
namespace Aventour.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IFavoritoRepository Favoritos { get; }

        // Agrega aquí los repositorios a medida que los vayas creando
        IDestinoRepository Destinos { get; }
        
        IResenaRepository  Resenas { get; }

        IAgenciaRepository Agencias { get; }  // <---- Agregar aquí
        // Ejemplos futuros:
        // IRutaRepository Rutas { get; }
        // IAgenciaRepository Agencias { get; }
        
        
        
        
        

        // Método para guardar todos los cambios en la base de datos de una sola vez
        Task<int> SaveChangesAsync();
        Task<int> CommitAsync();
    }
}