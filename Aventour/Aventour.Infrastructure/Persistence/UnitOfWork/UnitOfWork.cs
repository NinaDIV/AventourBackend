 
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Aventour.Infrastructure.Persistence.Repositories;
using Aventour.Infrastructure.Repositories;

namespace Aventour.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    
    {
        private readonly AventourDbContext _context;
        
        // Campos privados para lazy loading
        private IFavoritoRepository? _favoritos;
        // Implementación Lazy del Repositorio de Favoritos
        public IFavoritoRepository Favoritos => _favoritos ??= new FavoritoRepository(_context);
        // Método central para persistir todos los cambios
        public async Task<int> CommitAsync()
        {
            // EF Core realiza el seguimiento, y SaveChangesAsync ejecuta la transacción.
            return await _context.SaveChangesAsync();
        }
        private IDestinoRepository? _destinos;
        
        // Constructor with dependency injection
        public UnitOfWork(AventourDbContext context)
        {
            _context = context;
        }

        // Lazy initialization of repositories
        public IDestinoRepository Destinos => _destinos ??= new DestinoRepository(_context);

        // Save changes to the database
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose method for releasing resources
        public void Dispose()
        {
            // Dispose of the context and the repositories (if necessary)
            _context.Dispose();
           
            GC.SuppressFinalize(this);
        }
    }
}