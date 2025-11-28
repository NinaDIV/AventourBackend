// Aventour.Application/UseCases/Favorito/IFavoritoService.cs
using Aventour.Application.DTOs;
using Aventour.Domain.Enums; // Importante para el Enum

public interface IFavoritoService
{
    Task<List<FavoritoDto>> GetFavoritosDelUsuarioAsync(int idUsuario);
    Task<FavoritoDto> AddFavoritoAsync(int idUsuario, FavoritoCreateDto favoritoDto);
    
    // CORRECCIÓN 4: El borrado necesita saber qué tipo de entidad borrar
    Task<bool> RemoveFavoritoAsync(int idUsuario, int idEntidad, TipoFavorito tipo);
}