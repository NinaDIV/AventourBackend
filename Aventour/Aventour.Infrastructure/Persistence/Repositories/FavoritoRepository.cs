using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Persistence.Repositories
{
    public class FavoritoRepository : IFavoritoRepository
    {
        private readonly AventourDbContext _context;

        public FavoritoRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> ListarFavoritosPorUsuario(int idUsuario)
        {
            // Hacemos un JOIN explícito ya que no hay propiedad de navegación en la entidad Favorito hacia Destino
            var query = from fav in _context.Favoritos
                join dest in _context.DestinosTuristicos on fav.IdEntidad equals dest.IdDestino
                where fav.IdUsuario == idUsuario
                select new { Favorito = fav, Destino = dest };

            return await query.ToListAsync();
        }

        public async Task<bool> ExisteFavorito(int idUsuario, int idDestino)
        {
            return await _context.Favoritos
                .AnyAsync(f => f.IdUsuario == idUsuario && f.IdEntidad == idDestino);
        }

        public async Task AgregarFavorito(Favorito favorito)
        {
            await _context.Favoritos.AddAsync(favorito);
        }

        public async Task EliminarFavorito(int idUsuario, int idDestino)
        {
            var favorito = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.IdUsuario == idUsuario && f.IdEntidad == idDestino);

            if (favorito != null)
            {
                _context.Favoritos.Remove(favorito);
            }
        }
    }
}