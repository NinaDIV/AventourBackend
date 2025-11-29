using Aventour.Application.DTOs.Packs;
using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;

namespace Aventour.Application.Services.Packs
{
    public class PackRutaService
    {
        private readonly IPackRutaRepository _packRepository;

        public PackRutaService(IPackRutaRepository packRepository)
        {
            _packRepository = packRepository;
        }

        // Helper para validar si el usuario es agencia
        private async Task<int> ValidarEsAgencia(string emailUsuario)
        {
            var idAgencia = await _packRepository.GetAgenciaIdByEmailAsync(emailUsuario);
            if (idAgencia == null)
            {
                throw new UnauthorizedAccessException("El usuario autenticado no está registrado como una Agencia válida.");
            }
            return idAgencia.Value;
        }

        // 1. CREAR PACK
        public async Task<int> CrearPackAsync(string emailUsuario, CrearPackDto dto)
        {
            // Validar rol de agencia
            int idAgencia = await ValidarEsAgencia(emailUsuario);

            var nuevoPack = new PacksRutasAgencium
            {
                IdAgencia = idAgencia,
                NombrePack = dto.NombrePack,
                PrecioBase = dto.PrecioBase,
                Descripcion = dto.Descripcion,
                DuracionDias = dto.DuracionDias
            };

            var packCreado = await _packRepository.AddAsync(nuevoPack);

            if (dto.Destinos != null && dto.Destinos.Any())
            {
                var detalles = dto.Destinos.Select(d => new DetallePackDestino
                {
                    IdPack = packCreado.IdPack,
                    IdDestino = d.IdDestino,
                    OrdenParada = d.OrdenParada,
                    NotasDia = d.NotasDia
                });
                await _packRepository.AddDetallesAsync(detalles);
            }

            return packCreado.IdPack;
        }

        // 2. OBTENER TODOS (Público)
        public async Task<IEnumerable<PackResponseDto>> ListarTodosAsync()
        {
            var packs = await _packRepository.GetAllAsync();

            return packs.Select(p => new PackResponseDto
            {
                IdPack = p.IdPack,
                IdAgencia = p.IdAgencia,
                NombreAgencia = p.IdAgenciaNavigation?.Nombre ?? "Desconocida",
                NombrePack = p.NombrePack,
                PrecioBase = p.PrecioBase,
                Descripcion = p.Descripcion,
                DuracionDias = p.DuracionDias,
                // Mapeo ligero de destinos (opcional, si quieres listar detalles en la vista general)
                Destinos = p.DetallePackDestinos.Select(d => new DetallePackResponseDto
                {
                    IdDestino = d.IdDestino,
                    NombreDestino = d.IdDestinoNavigation?.Nombre ?? "Destino",
                    OrdenParada = d.OrdenParada
                }).OrderBy(x => x.OrdenParada).ToList()
            });
        }

        // 3. OBTENER DETALLE
        public async Task<PackResponseDto> ObtenerPorIdAsync(int id)
        {
            var pack = await _packRepository.GetByIdAsync(id);
            if (pack == null) throw new KeyNotFoundException("El pack no existe.");

            return new PackResponseDto
            {
                IdPack = pack.IdPack,
                IdAgencia = pack.IdAgencia,
                NombreAgencia = pack.IdAgenciaNavigation?.Nombre ?? "Desconocida",
                NombrePack = pack.NombrePack,
                PrecioBase = pack.PrecioBase,
                Descripcion = pack.Descripcion,
                DuracionDias = pack.DuracionDias,
                Destinos = pack.DetallePackDestinos.Select(d => new DetallePackResponseDto
                {
                    IdDetallePack = d.IdDetallePack,
                    IdDestino = d.IdDestino,
                    NombreDestino = d.IdDestinoNavigation?.Nombre ?? "Cargando...",
                    UrlFoto = d.IdDestinoNavigation?.UrlFotoPrincipal,
                    OrdenParada = d.OrdenParada,
                    NotasDia = d.NotasDia
                }).OrderBy(x => x.OrdenParada).ToList()
            };
        }

        // 4. ACTUALIZAR (Solo Agencia Propietaria)
        public async Task ActualizarPackAsync(int idPack, string emailUsuario, UpdatePackDto dto)
        {
            var pack = await _packRepository.GetByIdAsync(idPack);
            if (pack == null) throw new KeyNotFoundException("Pack no encontrado.");

            // Validar propiedad
            int idAgencia = await ValidarEsAgencia(emailUsuario);
            if (pack.IdAgencia != idAgencia)
            {
                throw new UnauthorizedAccessException("No tienes permiso para editar este pack.");
            }

            // Actualizar cabecera
            pack.NombrePack = dto.NombrePack;
            pack.PrecioBase = dto.PrecioBase;
            pack.Descripcion = dto.Descripcion;
            pack.DuracionDias = dto.DuracionDias;
            
            await _packRepository.UpdateAsync(pack);

            // Actualizar detalles: Estrategia simple (Borrar y Recrear)
            // Para mantener consistencia de orden
            await _packRepository.RemoveDetallesAsync(idPack);
            
            if (dto.Destinos != null && dto.Destinos.Any())
            {
                var nuevosDetalles = dto.Destinos.Select(d => new DetallePackDestino
                {
                    IdPack = idPack,
                    IdDestino = d.IdDestino,
                    OrdenParada = d.OrdenParada,
                    NotasDia = d.NotasDia
                });
                await _packRepository.AddDetallesAsync(nuevosDetalles);
            }
        }

        // 5. ELIMINAR (Solo Agencia Propietaria)
        public async Task EliminarPackAsync(int idPack, string emailUsuario)
        {
            var pack = await _packRepository.GetByIdAsync(idPack);
            if (pack == null) throw new KeyNotFoundException("Pack no encontrado.");

            int idAgencia = await ValidarEsAgencia(emailUsuario);
            if (pack.IdAgencia != idAgencia)
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar este pack.");
            }

            await _packRepository.DeleteAsync(pack);
        }
    }
}