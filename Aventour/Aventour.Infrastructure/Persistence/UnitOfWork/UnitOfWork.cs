using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Aventour.Infrastructure.Persistence.Repositories;

namespace Aventour.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AventourDbContext _context;
        
        // Variable privada para almacenar la instancia del repositorio
        private IDestinoRepository? _destinos;

        public UnitOfWork(AventourDbContext context)
        {
            _context = context;
        }

        // Implementación del Patrón Singleton por Request:
        // Solo crea la instancia del repositorio si se solicita.
        public IDestinoRepository Destinos
        {
            get
            {
                // Si _destinos es nulo, crea una nueva instancia inyectando el contexto actual.
                // Si ya existe, devuelve la misma instancia.
                return _destinos ??= new DestinoRepository(_context);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}