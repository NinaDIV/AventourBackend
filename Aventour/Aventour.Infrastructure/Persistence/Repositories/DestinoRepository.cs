using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Persistence.Repositories
{
    public class DestinoRepository : IDestinoRepository
    {
        private readonly AventourDbContext _context;

        public DestinoRepository(AventourDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DestinosTuristico>> ListarAsync()
        {
            return await _context.DestinosTuristicos
                                 .AsNoTracking() // Optimización para lectura
                                 .OrderBy(d => d.Nombre)
                                 .ToListAsync();
        }

        public async Task<DestinosTuristico?> ObtenerPorIdAsync(int id)
        {
            return await _context.DestinosTuristicos.FindAsync(id);
        }

        public async Task<int> CrearAsync(DestinosTuristico destino)
        {
            await _context.DestinosTuristicos.AddAsync(destino);
           // await _context.SaveChangesAsync();
            return destino.IdDestino;
        }

        public async Task ActualizarAsync(DestinosTuristico destino)
        {
            // En EF Core, si la entidad ya está trackeada, SaveChanges detecta los cambios.
            // Si viene desconectada, usamos Update.
            _context.DestinosTuristicos.Update(destino);
            //await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var destino = await _context.DestinosTuristicos.FindAsync(id);
            if (destino != null)
            {
                _context.DestinosTuristicos.Remove(destino);
               // await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DestinosTuristico>> BuscarPorNombreAsync(string nombre)
        {
            return await _context.DestinosTuristicos
                                 .AsNoTracking()
                                 .Where(d => d.Nombre.ToLower().Contains(nombre.ToLower()))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<DestinosTuristico>> BuscarCercanosAsync(decimal latitud, decimal longitud, double radioKm)
        {
            // Implementación simple en memoria para evitar complejidad de PostGIS por ahora.
            // NOTA: Para producción con muchos datos, usa consultas espaciales nativas de Postgres.
            var destinos = await _context.DestinosTuristicos.AsNoTracking().ToListAsync();
            
            // Fórmula simple de distancia euclidiana o similar (aproximación)
            return destinos.Where(d => 
            {
                double dist = CalcularDistancia((double)latitud, (double)longitud, (double)d.Latitud, (double)d.Longitud);
                return dist <= radioKm;
            }).ToList();
        }

        private double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            // Fórmula de Haversine simplificada
            var R = 6371; // Radio tierra km
            var dLat = (lat2 - lat1) * (Math.PI / 180);
            var dLon = (lon2 - lon1) * (Math.PI / 180);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}