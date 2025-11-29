using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Persistence.Repositories
{
    public class PackRutaRepository : IPackRutaRepository
    {
        private readonly AventourDbContext _context;

        public PackRutaRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<PacksRutasAgencium?> GetByIdAsync(int idPack)
        {
            return await _context.PacksRutasAgencia
                .Include(p => p.IdAgenciaNavigation) // Datos de la agencia
                .Include(p => p.DetallePackDestinos)
                    .ThenInclude(d => d.IdDestinoNavigation) // Datos de los destinos
                .FirstOrDefaultAsync(p => p.IdPack == idPack);
        }

        public async Task<IEnumerable<PacksRutasAgencium>> GetAllAsync()
        {
            return await _context.PacksRutasAgencia
                .Include(p => p.IdAgenciaNavigation)
                .Include(p => p.DetallePackDestinos)
                .ThenInclude(d => d.IdDestinoNavigation)
                .ToListAsync();
        }

        public async Task<PacksRutasAgencium> AddAsync(PacksRutasAgencium pack)
        {
            await _context.PacksRutasAgencia.AddAsync(pack);
            await _context.SaveChangesAsync();
            return pack;
        }

        public async Task UpdateAsync(PacksRutasAgencium pack)
        {
            _context.PacksRutasAgencia.Update(pack);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PacksRutasAgencium pack)
        {
            // Eliminar detalles primero manualmente (si no hay cascade en BD)
            var detalles = _context.DetallePackDestinos.Where(d => d.IdPack == pack.IdPack);
            _context.DetallePackDestinos.RemoveRange(detalles);

            _context.PacksRutasAgencia.Remove(pack);
            await _context.SaveChangesAsync();
        }

        public async Task AddDetallesAsync(IEnumerable<DetallePackDestino> detalles)
        {
            await _context.DetallePackDestinos.AddRangeAsync(detalles);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDetallesAsync(int idPack)
        {
            var detalles = _context.DetallePackDestinos.Where(d => d.IdPack == idPack);
            _context.DetallePackDestinos.RemoveRange(detalles);
            await _context.SaveChangesAsync();
        }

        public async Task<int?> GetAgenciaIdByEmailAsync(string email)
        {
            // Buscamos en la tabla AgenciasGuias si existe una agencia con este email
            var agencia = await _context.AgenciasGuias
                .FirstOrDefaultAsync(a => a.Email == email); // Asumiendo que el usuario y agencia comparten email
            
            return agencia?.IdAgencia;
        }
    }
}