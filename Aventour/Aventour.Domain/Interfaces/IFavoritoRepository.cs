// Aventour.Domain.Interfaces/IFavoritoRepository.cs
using Aventour.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aventour.Domain.Interfaces
{
    public interface IFavoritoRepository
    {
        Task<Favorito?> GetFavoritoAsync(int idUsuario, int idEntidad);
        Task<List<Favorito>> GetFavoritosByUsuarioAsync(int idUsuario);
        Task AddFavoritoAsync(Favorito favorito);
        Task DeleteFavoritoAsync(Favorito favorito);
        // Opcional: Para el CRUD completo, pero no necesario para esta entidad de cruce
        // Task UpdateFavoritoAsync(Favorito favorito); 
    }
}