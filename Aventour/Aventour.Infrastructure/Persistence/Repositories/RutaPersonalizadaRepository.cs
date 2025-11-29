using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Repositories
{
    public class RutaPersonalizadaRepository : IRutaPersonalizadaRepository
    {
        private readonly AventourDbContext _context;

        public RutaPersonalizadaRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<RutasPersonalizada?> GetByIdAsync(int idRuta)
        {
            return await _context.RutasPersonalizadas
                .Include(r => r.DetalleRuta)
                    .ThenInclude(d => d.IdDestinoNavigation) // Incluir datos del destino (Nombre, Foto)
                .FirstOrDefaultAsync(r => r.IdRuta == idRuta);
        }

        public async Task<IEnumerable<RutasPersonalizada>> GetByUserIdAsync(int idUsuario)
        {
            return await _context.RutasPersonalizadas
                .Where(r => r.IdUsuario == idUsuario)
                .Include(r => r.DetalleRuta)
                .ThenInclude(d => d.IdDestinoNavigation)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<RutasPersonalizada> AddAsync(RutasPersonalizada ruta)
        {
            await _context.RutasPersonalizadas.AddAsync(ruta);
            await _context.SaveChangesAsync();
            return ruta;
        }

        public async Task UpdateAsync(RutasPersonalizada ruta)
        {
            _context.RutasPersonalizadas.Update(ruta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RutasPersonalizada ruta)
        {
            // Primero eliminamos los detalles para evitar errores de FK si no hay CASCADE en BD
            // Aunque si configuraste ON DELETE CASCADE en BD, esto no sería estrictamente necesario,
            // pero es buena práctica en EF Core.
            var detalles = _context.DetalleRutas.Where(d => d.IdRuta == ruta.IdRuta);
            _context.DetalleRutas.RemoveRange(detalles);

            _context.RutasPersonalizadas.Remove(ruta);
            await _context.SaveChangesAsync();
        }

        public async Task AddDetallesAsync(IEnumerable<DetalleRuta> detalles)
        {
            await _context.DetalleRutas.AddRangeAsync(detalles);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDetallesAsync(int idRuta)
        {
            var detalles = _context.DetalleRutas.Where(d => d.IdRuta == idRuta);
            _context.DetalleRutas.RemoveRange(detalles);
            await _context.SaveChangesAsync();
        }
    }
}