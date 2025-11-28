// Aventour.Domain/Interfaces/IFavoritoRepository.cs
using Aventour.Domain.Models;
using Aventour.Domain.Enums; // Asegúrate de importar el Enum

public interface IFavoritoRepository
{
    // CORRECCIÓN 2: Añadir el parámetro TipoFavorito para diferenciar Destino de Lugar
    Task<List<Favorito>> GetFavoritosByUsuarioAsync(int idUsuario);
    Task<Favorito?> GetFavoritoAsync(int idUsuario, int idEntidad, TipoFavorito tipo);
    Task AddFavoritoAsync(Favorito favorito);
    Task DeleteFavoritoAsync(Favorito favorito);
}