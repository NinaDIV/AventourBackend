using Aventour.Application.DTOs.Favoritos;

namespace Aventour.Application.Interfaces
{
    public interface IFavoritoService
    {
        Task<IEnumerable<FavoritoResponseDto>> ObtenerFavoritosUsuario(int idUsuario);
        Task<bool> AgregarFavorito(int idUsuario, CrearFavoritoDto dto);
        Task<bool> EliminarFavorito(int idUsuario, int idDestino);
    }
}