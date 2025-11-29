using Aventour.Domain.Entities;

 

namespace Aventour.Application.UseCases.Destinos
{
    public interface IGestionarDestinosUseCase
    {
        Task<int> CrearDestino(DestinosTuristico destino);
        Task ActualizarDestino(DestinosTuristico destino);
        Task EliminarDestino(int id);
        Task ActualizarPuntuacionMedia(int id);
    }

    public interface IConsultarDestinosUseCase
    {
        Task<IEnumerable<DestinosTuristico>> ListarDestinos();
        Task<DestinosTuristico> ObtenerDestino(int id);
        Task<IEnumerable<DestinosTuristico>> BuscarDestinos(string nombre);
    }
}