using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aventour.Infrastructure.Repositories
{
    public class HotelRestauranteRepository : IHotelRestauranteRepository
    {
        private readonly AventourDbContext _context;

        public HotelRestauranteRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<HotelesRestaurante?> GetByIdAsync(int id)
        {
            return await _context.HotelesRestaurantes.FindAsync(id);
        }

        public async Task<IEnumerable<HotelesRestaurante>> GetAllAsync(string? tipo = null)
        {
            var query = _context.HotelesRestaurantes.AsQueryable();

            // Si se pasa un tipo, filtramos. Nota: Esto requiere que la entidad tenga la propiedad 'Tipo'
            // if (!string.IsNullOrEmpty(tipo))
            //    query = query.Where(h => h.Tipo == tipo);

            return await query.ToListAsync();
        }

        public async Task<HotelesRestaurante> AddAsync(HotelesRestaurante entidad)
        {
            await _context.HotelesRestaurantes.AddAsync(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }

        public async Task UpdateAsync(HotelesRestaurante entidad)
        {
            _context.HotelesRestaurantes.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(HotelesRestaurante entidad)
        {
            _context.HotelesRestaurantes.Remove(entidad);
            await _context.SaveChangesAsync();
        }
    }
}