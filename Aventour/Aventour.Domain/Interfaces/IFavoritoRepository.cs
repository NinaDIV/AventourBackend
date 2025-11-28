using Aventour.Domain.Entities;

namespace Aventour.Domain.Interfaces
{
    public interface IFavoritoRepository
    {
        // Devuelve una tupla o clase con info del favorito y del destino
        // Dado que Favorito no tiene navegación directa a Destino en tu entidad actual,
        // devolveremos una estructura combinada o el Favorito puro y haremos join en Infra.
        Task<IEnumerable<dynamic>> ListarFavoritosPorUsuario(int idUsuario);
        Task AgregarFavorito(Favorito favorito);
        Task EliminarFavorito(int idUsuario, int idDestino);
        Task<bool> ExisteFavorito(int idUsuario, int idDestino);
    }
}