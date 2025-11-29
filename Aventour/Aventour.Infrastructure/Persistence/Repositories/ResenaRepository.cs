using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Aventour.Domain.Enums;

namespace Aventour.Infrastructure.Persistence.Repositories
{
    public class ResenaRepository : IResenaRepository
    {
        private readonly AventourDbContext _context;

        public ResenaRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<List<Resena>> GetResenasByEntidadAsync(int idEntidad, TipoResena tipoEntidad)
        {
            // Convertimos el Enum a string explícitamente si EF no lo hace automático en la consulta
            // Pero como configuramos el conversion, pasamos el enum directo.
            return await _context.Resenas
                .Include(r => r.IdUsuarioNavigation) // Incluimos usuario para mostrar el nombre si es necesario
                .Where(r => r.IdEntidad == idEntidad && r.TipoEntidad == tipoEntidad.ToString()) 
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Resena>> GetResenasByUsuarioAsync(int idUsuario)
        {
            return await _context.Resenas
                .Where(r => r.IdUsuario == idUsuario)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Resena?> GetResenaEspecificaAsync(int idUsuario, int idEntidad, TipoResena tipoEntidad)
        {
            return await _context.Resenas
                .FirstOrDefaultAsync(r => r.IdUsuario == idUsuario && 
                                          r.IdEntidad == idEntidad && 
                                          r.TipoEntidad == tipoEntidad.ToString());
        }

        public async Task<Resena?> GetByIdAsync(int idResena)
        {
            return await _context.Resenas.FindAsync(idResena);
        }

        public async Task AddResenaAsync(Resena resena)
        {
            await _context.Resenas.AddAsync(resena);
        }

        public Task DeleteResenaAsync(Resena resena)
        {
            _context.Resenas.Remove(resena);
            return Task.CompletedTask;
        }
    }
}