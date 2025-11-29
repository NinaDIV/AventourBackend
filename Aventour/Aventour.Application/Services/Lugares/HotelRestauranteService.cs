using Aventour.Application.DTOs.Lugares;
using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aventour.Application.Services
{
    public class HotelRestauranteService
    {
        private readonly IHotelRestauranteRepository _repository;

        public HotelRestauranteService(IHotelRestauranteRepository repository)
        {
            _repository = repository;
        }

        // Listar (Opcionalmente filtrando por tipo)
        public async Task<IEnumerable<LugarResponseDto>> ListarLugaresAsync(string? tipo = null)
        {
            var lugares = await _repository.GetAllAsync(tipo);
            
            return lugares.Select(l => new LugarResponseDto
            {
                IdLugar = l.IdLugar,
                Nombre = l.Nombre,
                // Nota: Asegúrate de agregar la propiedad 'Tipo' a tu entidad HotelesRestaurante si falta
                // l.Tipo vendría de la entidad. Si no lo tienes en la clase C#, agrégalo: public string Tipo {get;set;}
                Tipo = "Consultar Entidad", 
                Latitud = l.Latitud,
                Longitud = l.Longitud,
                Direccion = l.Direccion,
                PuntuacionMedia = l.PuntuacionMedia
            });
        }

        public async Task<LugarResponseDto> ObtenerPorIdAsync(int id)
        {
            var l = await _repository.GetByIdAsync(id);
            if (l == null) throw new KeyNotFoundException("Lugar no encontrado.");

            return new LugarResponseDto
            {
                IdLugar = l.IdLugar,
                Nombre = l.Nombre,
                // Tipo = l.Tipo,
                Latitud = l.Latitud,
                Longitud = l.Longitud,
                Direccion = l.Direccion,
                PuntuacionMedia = l.PuntuacionMedia
            };
        }

        public async Task<int> CrearLugarAsync(CrearLugarDto dto)
        {
            var nuevoLugar = new HotelesRestaurante
            {
                Nombre = dto.Nombre,
                // Tipo = dto.Tipo, // IMPORTANTE: Descomentar cuando actualices la entidad
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                Direccion = dto.Direccion,
                PuntuacionMedia = 0 // Inicializar en 0
            };

            var creado = await _repository.AddAsync(nuevoLugar);
            return creado.IdLugar;
        }

        public async Task ActualizarLugarAsync(int id, UpdateLugarDto dto)
        {
            var lugar = await _repository.GetByIdAsync(id);
            if (lugar == null) throw new KeyNotFoundException("Lugar no encontrado.");

            lugar.Nombre = dto.Nombre;
            lugar.Latitud = dto.Latitud;
            lugar.Longitud = dto.Longitud;
            lugar.Direccion = dto.Direccion;
            // lugar.Tipo = dto.Tipo;

            await _repository.UpdateAsync(lugar);
        }

        public async Task EliminarLugarAsync(int id)
        {
            var lugar = await _repository.GetByIdAsync(id);
            if (lugar == null) throw new KeyNotFoundException("Lugar no encontrado.");

            await _repository.DeleteAsync(lugar);
        }
    }
}