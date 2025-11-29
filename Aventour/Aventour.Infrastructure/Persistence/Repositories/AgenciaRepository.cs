using Aventour.Domain.Entities;
using Aventour.Domain.Enums;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Persistence.Repositories
{
    public class AgenciaRepository : IAgenciaRepository
    {
        private readonly AventourDbContext _context;

        public AgenciaRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<List<AgenciasGuia>> GetAllAsync(TipoAgenciaGuia? filtroTipo)
        {
            var query = _context.AgenciasGuias.AsQueryable();

            if (filtroTipo.HasValue)
            {
                // HasConversion en DbContext se encarga del Enum <-> String
                query = query.Where(x => x.Tipo == filtroTipo.Value);
            }

            return await query.OrderBy(x => x.Nombre).ToListAsync();
        }

        public async Task<AgenciasGuia?> GetByIdAsync(int id)
        {
            return await _context.AgenciasGuias.FindAsync(id);
        }

        public async Task AddAsync(AgenciasGuia entidad)
        {
            await _context.AgenciasGuias.AddAsync(entidad);
        }

        public Task UpdateAsync(AgenciasGuia entidad)
        {
            _context.AgenciasGuias.Update(entidad);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(AgenciasGuia entidad)
        {
      
            _context.AgenciasGuias.Remove(entidad);
            return Task.CompletedTask;
        }
        public async Task RecalcularPuntuacionMediaAsync(int idAgencia)
        {
            // 1. Calcular el promedio. 
            var promedio = await _context.Resenas
                .Where(r => r.IdEntidad == idAgencia && 
                            (r.TipoEntidad == TipoResena.Agencia || r.TipoEntidad == TipoResena.Guia)) // Conversión correcta al enum
                .AverageAsync(r => (double?)r.Puntuacion);
           // <--- El truco es (double?)

            // 2. Si es null (no hay reseñas), ponemos 0.
            double valorFinal = promedio ?? 0.0;

            // 3. Obtener la entidad agencia para actualizarla
            var agencia = await _context.AgenciasGuias.FindAsync(idAgencia);
            
            if (agencia != null)
            {
                // 4. Actualizar el campo y redondear a 1 decimal
                agencia.PuntuacionMedia = (decimal)Math.Round(valorFinal, 1);
                
                // NOTA: No necesitamos llamar a _context.SaveChanges() aquí explícitamente 
                // si este método es llamado dentro de un flujo que termina en UnitOfWork.Commit().
                // Pero si se llama aislado, el UnitOfWork se encargará de guardar.
            }
        }

        
    }
}