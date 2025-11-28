using Aventour.Domain.Entities;
 

namespace Aventour.Domain.Interfaces
{
    public interface IDestinoRepository
    {
        Task<IEnumerable<DestinosTuristico>> ListarAsync();
        Task<DestinosTuristico?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(DestinosTuristico destino);
        Task ActualizarAsync(DestinosTuristico destino);
        Task EliminarAsync(int id);
        
        // Búsquedas específicas
        Task<IEnumerable<DestinosTuristico>> BuscarPorNombreAsync(string nombre);
        // Opcional: Buscar destinos en un radio de KM (lógica simple)
        Task<IEnumerable<DestinosTuristico>> BuscarCercanosAsync(decimal latitud, decimal longitud, double radioKm);
        Task<DestinosTuristico?> GetByIdAsync(int dtoIdEntidad);


    }
}