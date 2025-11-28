using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;

namespace Aventour.Application.UseCases.Destinos
{
    // CASOS DE USO DE COMANDOS (Escritura)
    public class GestionarDestinosUseCase : IGestionarDestinosUseCase
    {
        private readonly IDestinoRepository _repository;

        public GestionarDestinosUseCase(IDestinoRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CrearDestino(DestinosTuristico destino)
        {
            // Aquí podrías validar reglas de negocio (ej. no duplicar nombres exactos)
            var existentes = await _repository.BuscarPorNombreAsync(destino.Nombre);
            if (existentes.Any()) throw new Exception("El destino ya existe.");

            return await _repository.CrearAsync(destino);
        }

        public async Task ActualizarDestino(DestinosTuristico destino)
        {
            var existente = await _repository.ObtenerPorIdAsync(destino.IdDestino);
            if (existente == null) throw new Exception("Destino no encontrado.");

            // Actualizamos campos
            existente.Nombre = destino.Nombre;
            existente.DescripcionBreve = destino.DescripcionBreve;
            existente.DescripcionCompleta = destino.DescripcionCompleta;
            existente.Tipo = destino.Tipo;
            existente.Latitud = destino.Latitud;
            existente.Longitud = destino.Longitud;
            existente.HorarioAtencion = destino.HorarioAtencion;
            existente.CostoEntrada = destino.CostoEntrada;
            existente.UrlFotoPrincipal = destino.UrlFotoPrincipal;

            await _repository.ActualizarAsync(existente);
        }

        public async Task EliminarDestino(int id)
        {
            await _repository.EliminarAsync(id);
        }
    }

    // CASOS DE USO DE CONSULTAS (Lectura)
    public class ConsultarDestinosUseCase : IConsultarDestinosUseCase
    {
        private readonly IDestinoRepository _repository;

        public ConsultarDestinosUseCase(IDestinoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DestinosTuristico>> ListarDestinos()
        {
            return await _repository.ListarAsync();
        }

        public async Task<DestinosTuristico> ObtenerDestino(int id)
        {
            var destino = await _repository.ObtenerPorIdAsync(id);
            if (destino == null) throw new KeyNotFoundException($"No se encontró el destino con ID {id}");
            return destino;
        }

        public async Task<IEnumerable<DestinosTuristico>> BuscarDestinos(string nombre)
        {
            return await _repository.BuscarPorNombreAsync(nombre);
        }
    }
}