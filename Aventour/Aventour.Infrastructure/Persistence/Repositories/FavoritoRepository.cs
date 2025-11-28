// Aventour.Infrastructure.Persistence.Repositories/FavoritoRepository.cs
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aventour.Domain.Enums;

namespace Aventour.Infrastructure.Persistence.Repositories
{
    public class FavoritoRepository : IFavoritoRepository
    {
        private readonly AventourDbContext _context;

        public FavoritoRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<List<Favorito>> GetFavoritosByUsuarioAsync(int idUsuario)
        {
            return await _context.Favoritos
                .Where(f => f.IdUsuario == idUsuario)
                .OrderByDescending(f => f.FechaGuardado) // Los más recientes primero
                .ToListAsync();
        }

        public async Task<Favorito?> GetFavoritoAsync(Int32 idUsuario, Int32 idEntidad, TipoFavorito tipo)
        {
            return await _context.Favoritos
                .FirstOrDefaultAsync(f => f.IdUsuario == idUsuario && 
                                          f.IdEntidad == idEntidad && 
                                          f.TipoEntidad == tipo);
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