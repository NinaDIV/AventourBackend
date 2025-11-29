using Aventour.Application.DTOs.Rutas;
using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aventour.Application.Services
{
    public class RutaPersonalizadaService
    {
        private readonly IRutaPersonalizadaRepository _rutaRepository;
        // Inyectamos repositorio de destinos si necesitamos validar que existan, 
        // aunque EF se quejará por Foreign Key si no existen.

        public RutaPersonalizadaService(IRutaPersonalizadaRepository rutaRepository)
        {
            _rutaRepository = rutaRepository;
        }

        // 1. CREAR RUTA
        public async Task<int> CrearRutaAsync(int idUsuario, CrearRutaDto dto)
        {
            var nuevaRuta = new RutasPersonalizada
            {
                IdUsuario = idUsuario,
                NombreRuta = dto.NombreRuta,
                IsPublica = dto.IsPublica,
                FechaCreacion = DateTime.Now
            };

            // Guardamos la cabecera para obtener el ID
            var rutaCreada = await _rutaRepository.AddAsync(nuevaRuta);

            // Si vienen destinos iniciales, los agregamos
            if (dto.Destinos != null && dto.Destinos.Any())
            {
                var detalles = dto.Destinos.Select(d => new DetalleRuta
                {
                    IdRuta = rutaCreada.IdRuta,
                    IdDestino = d.IdDestino,
                    OrdenParada = d.OrdenParada,
                    Notas = d.Notas
                }).ToList();

                await _rutaRepository.AddDetallesAsync(detalles);
            }

            return rutaCreada.IdRuta;
        }

        // 2. OBTENER RUTAS DEL USUARIO (GET)
        public async Task<IEnumerable<RutaResponseDto>> ObtenerRutasPorUsuarioAsync(int idUsuario)
        {
            var rutas = await _rutaRepository.GetByUserIdAsync(idUsuario);
            
            // Mapeo a DTO
            return rutas.Select(r => new RutaResponseDto
            {
                IdRuta = r.IdRuta,
                IdUsuario = r.IdUsuario,
                NombreRuta = r.NombreRuta,
                FechaCreacion = r.FechaCreacion,
                IsPublica = r.IsPublica ?? false,
                // Nota: En la lista general, quizás no necesites cargar todos los detalles 
                // para no hacer la consulta muy pesada, aquí retornamos la lista básica o con count.
                Detalles = r.DetalleRuta.Select(d => new DetalleRutaResponseDto
                {
                    IdDetalle = d.IdDetalle,
                    IdDestino = d.IdDestino,
                    NombreDestino = d.IdDestinoNavigation?.Nombre ?? "Desconocido",
                    OrdenParada = d.OrdenParada
                }).OrderBy(x => x.OrdenParada).ToList()
            });
        }

        // 3. OBTENER DETALLE DE UNA RUTA (GET ID)
        public async Task<RutaResponseDto> ObtenerDetalleRutaAsync(int idRuta, int idUsuarioSolicitante)
        {
            var ruta = await _rutaRepository.GetByIdAsync(idRuta);
            if (ruta == null) throw new KeyNotFoundException("La ruta no existe.");

            // Validar visibilidad: El dueño puede verla, o cualquiera si es pública
            bool esPropietario = ruta.IdUsuario == idUsuarioSolicitante;
            if (!esPropietario && (ruta.IsPublica != true))
            {
                throw new UnauthorizedAccessException("No tienes permiso para ver esta ruta privada.");
            }

            return new RutaResponseDto
            {
                IdRuta = ruta.IdRuta,
                IdUsuario = ruta.IdUsuario,
                NombreRuta = ruta.NombreRuta,
                FechaCreacion = ruta.FechaCreacion,
                IsPublica = ruta.IsPublica ?? false,
                Detalles = ruta.DetalleRuta.Select(d => new DetalleRutaResponseDto
                {
                    IdDetalle = d.IdDetalle,
                    IdDestino = d.IdDestino,
                    NombreDestino = d.IdDestinoNavigation?.Nombre ?? "Cargando...",
                    UrlFoto = d.IdDestinoNavigation?.UrlFotoPrincipal,
                    OrdenParada = d.OrdenParada,
                    Notas = d.Notas
                })
                .OrderBy(x => x.OrdenParada)
                .ToList()
            };
        }

        // 4. EDITAR RUTA (PUT)
        public async Task EditarRutaAsync(int idRuta, int idUsuario, UpdateRutaDto dto)
        {
            var ruta = await _rutaRepository.GetByIdAsync(idRuta);
            if (ruta == null) throw new KeyNotFoundException("Ruta no encontrada.");

            // Validar que sea el propietario
            if (ruta.IdUsuario != idUsuario)
                throw new UnauthorizedAccessException("Solo el creador puede editar la ruta.");

            ruta.NombreRuta = dto.NombreRuta;
            ruta.IsPublica = dto.IsPublica;

            await _rutaRepository.UpdateAsync(ruta);
        }

        // 5. ELIMINAR RUTA (DELETE)
        public async Task EliminarRutaAsync(int idRuta, int idUsuario)
        {
            var ruta = await _rutaRepository.GetByIdAsync(idRuta);
            if (ruta == null) throw new KeyNotFoundException("Ruta no encontrada.");

            if (ruta.IdUsuario != idUsuario)
                throw new UnauthorizedAccessException("Solo el creador puede eliminar la ruta.");

            // El repositorio se encargará de eliminar detalles por cascada o manualmente
            await _rutaRepository.DeleteAsync(ruta);
        }
        
        // Método extra: Agregar destinos a una ruta existente
        public async Task AgregarDestinosARutaAsync(int idRuta, int idUsuario, List<CrearDetalleRutaDto> nuevosDestinos)
        {
             var ruta = await _rutaRepository.GetByIdAsync(idRuta);
             if (ruta == null) throw new KeyNotFoundException("Ruta no encontrada.");
             if (ruta.IdUsuario != idUsuario) throw new UnauthorizedAccessException("No autorizado.");

             var detalles = nuevosDestinos.Select(d => new DetalleRuta
             {
                 IdRuta = idRuta,
                 IdDestino = d.IdDestino,
                 OrdenParada = d.OrdenParada,
                 Notas = d.Notas
             });

             await _rutaRepository.AddDetallesAsync(detalles);
        }
    }
}