using Aventour.Domain.Interfaces;
using Aventour.Domain.Entities;
using Aventour.Domain.Enums;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aventour.Application.Interfaces;

namespace Aventour.Infrastructure.Persistence.Repositories
{
    // ADAPTADOR: Implementa el Puerto IFavoritoRepository
    public class FavoritoRepository : IFavoritoRepository
    {
        private readonly AventourDbContext _context;

        public FavoritoRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<Favorito> AddAsync(Favorito favorito)
        {
            await _context.Favoritos.AddAsync(favorito);
            return favorito;
            // Nota: No se llama SaveChanges aquí, eso lo hace el UnitOfWork (Commit).
        }

        public void Remove(Favorito favorito)
        {
            _context.Favoritos.Remove(favorito);
            // Nota: No se llama SaveChanges aquí, eso lo hace el UnitOfWork (Commit).
        }

        public async Task<Favorito?> GetByIdAsync(int idUsuario, int idEntidad, TipoFavorito tipoEntidad)
        {
            // Búsqueda por llave compuesta
            return await _context.Favoritos
                .Where(f => f.IdUsuario == idUsuario && 
                            f.IdEntidad == idEntidad && 
                            f.TipoEntidad == tipoEntidad)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Favorito>> GetByUserIdAsync(int idUsuario)
        {
            return await _context.Favoritos
                .Where(f => f.IdUsuario == idUsuario)
                // Incluimos la navegación a Usuario si fuera necesario mostrar datos del usuario
                // .Include(f => f.IdUsuarioNavigation) 
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int idUsuario, int idEntidad, TipoFavorito tipoEntidad)
        {
            return await _context.Favoritos
                .AnyAsync(f => f.IdUsuario == idUsuario && 
                               f.IdEntidad == idEntidad && 
                               f.TipoEntidad == tipoEntidad);
        }
    }
}