using Aventour.Application.DTOs;
using Aventour.Domain.Enums; // Asegúrate de tener este using

namespace Aventour.Application.Services.Favoritos
{
    public interface IFavoritoService
    {
        Task<List<FavoritoDto>> GetFavoritosDelUsuarioAsync(int idUsuario);
        
        Task<FavoritoDto> AddFavoritoAsync(int idUsuario, FavoritoCreateDto dto);
        
        Task<bool> RemoveFavoritoAsync(int idUsuario, int idEntidad, TipoFavorito tipo);

        // --- NUEVOS MÉTODOS ---
        
        // Obtener un favorito específico
        Task<FavoritoDto> GetFavoritoByIdAsync(int idUsuario, int idEntidad, TipoFavorito tipo);
        
        // Actualizar un favorito (ej. refrescar fecha)
        Task UpdateFavoritoAsync(int idUsuario, int idEntidad, TipoFavorito tipo);
    }
}