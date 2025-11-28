using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;

namespace Aventour.Application.UseCases.Destinos
{
    public class ConsultarDestinosUseCase : IConsultarDestinosUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConsultarDestinosUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DestinosTuristico>> ListarDestinos()
        {
            // Accedemos a través de UoW
            return await _unitOfWork.Destinos.ListarAsync();
        }

        public async Task<DestinosTuristico> ObtenerDestino(int id)
        {
            var destino = await _unitOfWork.Destinos.ObtenerPorIdAsync(id);
            if (destino == null) throw new KeyNotFoundException($"No se encontró el destino con ID {id}");
            return destino;
        }

        public async Task<IEnumerable<DestinosTuristico>> BuscarDestinos(string nombre)
        {
            return await _unitOfWork.Destinos.BuscarPorNombreAsync(nombre);
        }
    }
}