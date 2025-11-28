using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;

namespace Aventour.Application.UseCases.Destinos
{
    public class GestionarDestinosUseCase : IGestionarDestinosUseCase
    {
        // 1. Inyectamos IUnitOfWork, NO el repositorio directo
        private readonly IUnitOfWork _unitOfWork;

        public GestionarDestinosUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CrearDestino(DestinosTuristico destino)
        {
            // Validaciones usando el repositorio a través del UoW
            var existentes = await _unitOfWork.Destinos.BuscarPorNombreAsync(destino.Nombre);
            if (existentes.Any()) throw new Exception("El destino ya existe.");

            // 2. Preparamos la creación (esto no guarda en BD todavía)
            await _unitOfWork.Destinos.CrearAsync(destino);

            // 3. AQUÍ guardamos los cambios definitivamente
            await _unitOfWork.SaveChangesAsync();

            return destino.IdDestino;
        }

        public async Task ActualizarDestino(DestinosTuristico destino)
        {
            var existente = await _unitOfWork.Destinos.ObtenerPorIdAsync(destino.IdDestino);
            if (existente == null) throw new Exception("Destino no encontrado.");

            // Mapeo de campos
            existente.Nombre = destino.Nombre;
            existente.DescripcionBreve = destino.DescripcionBreve;
            existente.DescripcionCompleta = destino.DescripcionCompleta;
            existente.Tipo = destino.Tipo;
            existente.Latitud = destino.Latitud;
            existente.Longitud = destino.Longitud;
            existente.HorarioAtencion = destino.HorarioAtencion;
            existente.CostoEntrada = destino.CostoEntrada;
            existente.UrlFotoPrincipal = destino.UrlFotoPrincipal;

            // Preparamos la actualización
            await _unitOfWork.Destinos.ActualizarAsync(existente);

            // Guardamos cambios
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EliminarDestino(int id)
        {
            // Preparamos la eliminación
            await _unitOfWork.Destinos.EliminarAsync(id);

            // Guardamos cambios
            await _unitOfWork.SaveChangesAsync();
        }
    }
}