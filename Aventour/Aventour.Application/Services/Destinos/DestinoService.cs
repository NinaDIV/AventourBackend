using Aventour.Application.DTOs.Destinos;
using Aventour.Application.UseCases.Destinos;
using Aventour.Domain.Entities;

namespace Aventour.Application.Services.Destinos
{
    public interface IDestinoService
    {
        Task<IEnumerable<DestinoResponseDto>> ListarTodos();
        Task<DestinoResponseDto> ObtenerPorId(int id);
        Task<int> Crear(CrearDestinoDto dto);
        Task Actualizar(UpdateDestinoDto dto);
        Task Eliminar(int id);
    }

    public class DestinoService : IDestinoService
    {
        private readonly IGestionarDestinosUseCase _gestionarUseCase;
        private readonly IConsultarDestinosUseCase _consultarUseCase;

        public DestinoService(IGestionarDestinosUseCase gestionarUseCase, IConsultarDestinosUseCase consultarUseCase)
        {
            _gestionarUseCase = gestionarUseCase;
            _consultarUseCase = consultarUseCase;
        }

        public async Task<IEnumerable<DestinoResponseDto>> ListarTodos()
        {
            var entidades = await _consultarUseCase.ListarDestinos();
            // Mapeo manual (o usar AutoMapper)
            return entidades.Select(e => new DestinoResponseDto
            {
                IdDestino = e.IdDestino,
                Nombre = e.Nombre,
                DescripcionBreve = e.DescripcionBreve,
                Tipo = e.Tipo,
                Latitud = e.Latitud,
                Longitud = e.Longitud,
                CostoEntrada = e.CostoEntrada,
                UrlFotoPrincipal = e.UrlFotoPrincipal,
                PuntuacionMedia = e.PuntuacionMedia
            });
        }

        public async Task<DestinoResponseDto> ObtenerPorId(int id)
        {
            var e = await _consultarUseCase.ObtenerDestino(id);
            return new DestinoResponseDto
            {
                IdDestino = e.IdDestino,
                Nombre = e.Nombre,
                DescripcionBreve = e.DescripcionBreve,
                Tipo = e.Tipo,
                Latitud = e.Latitud,
                Longitud = e.Longitud,
                CostoEntrada = e.CostoEntrada,
                UrlFotoPrincipal = e.UrlFotoPrincipal,
                PuntuacionMedia = e.PuntuacionMedia
            };
        }

        public async Task<int> Crear(CrearDestinoDto dto)
        {
            var entidad = new DestinosTuristico
            {
                Nombre = dto.Nombre,
                DescripcionBreve = dto.DescripcionBreve,
                DescripcionCompleta = dto.DescripcionCompleta,
                Tipo = dto.Tipo,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                HorarioAtencion = dto.HorarioAtencion,
                CostoEntrada = dto.CostoEntrada,
                UrlFotoPrincipal = dto.UrlFotoPrincipal
            };
            return await _gestionarUseCase.CrearDestino(entidad);
        }

        public async Task Actualizar(UpdateDestinoDto dto)
        {
            var entidad = new DestinosTuristico
            {
                IdDestino = dto.IdDestino,
                Nombre = dto.Nombre,
                DescripcionBreve = dto.DescripcionBreve,
                DescripcionCompleta = dto.DescripcionCompleta,
                Tipo = dto.Tipo,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                HorarioAtencion = dto.HorarioAtencion,
                CostoEntrada = dto.CostoEntrada,
                UrlFotoPrincipal = dto.UrlFotoPrincipal
            };
            await _gestionarUseCase.ActualizarDestino(entidad);
        }

        public async Task Eliminar(int id)
        {
            await _gestionarUseCase.EliminarDestino(id);
        }
    }
}