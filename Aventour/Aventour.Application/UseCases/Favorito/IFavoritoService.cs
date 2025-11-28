// Aventour.Application.UseCases.Favoritos/IFavoritoService.cs
using Aventour.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aventour.Application.UseCases.Favoritos
{
    public interface IFavoritoService
    {
        Task<List<FavoritoDto>> GetFavoritosDelUsuarioAsync(int idUsuario);
        Task<FavoritoDto> AddFavoritoAsync(int idUsuario, FavoritoCreateDto favoritoDto);
        Task<bool> RemoveFavoritoAsync(int idUsuario, int idEntidad);
    }
}