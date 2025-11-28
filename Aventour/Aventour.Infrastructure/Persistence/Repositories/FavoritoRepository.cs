// Aventour.Infrastructure.Persistence.Repositories/FavoritoRepository.cs
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aventour.Infrastructure.Persistence.Repositories
{
    public class FavoritoRepository : IFavoritoRepository
    {
        private readonly AventourDbContext _context;

        public FavoritoRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<Favorito?> GetFavoritoAsync(int idUsuario, int idEntidad)
        {
            return await _context.Favoritos
                .FirstOrDefaultAsync(f => f.IdUsuario == idUsuario && f.IdEntidad == idEntidad);
        }

        public async Task<List<Favorito>> GetFavoritosByUsuarioAsync(int idUsuario)
        {
            return await _context.Favoritos
                .Where(f => f.IdUsuario == idUsuario)
                // Incluye la navegación si es necesaria
                // .Include(f => f.IdUsuarioNavigation) 
                .ToListAsync();
        }

        public async Task AddFavoritoAsync(Favorito favorito)
        {
            await _context.Favoritos.AddAsync(favorito);
        }

        public Task DeleteFavoritoAsync(Favorito favorito)
        {
            _context.Favoritos.Remove(favorito);
            return Task.CompletedTask;
        }
    }
}