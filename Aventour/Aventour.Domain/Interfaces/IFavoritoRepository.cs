using Aventour.Domain.Entities;
using Aventour.Domain.Enums;

namespace Aventour.Application.Interfaces
{
    public interface IFavoritoRepository
    {
        // C - CREATE
        Task<Favorito> AddAsync(Favorito favorito);

        // R - READ
        Task<Favorito?> GetByIdAsync(int idUsuario, int idEntidad, TipoFavorito tipoEntidad);
        Task<IEnumerable<Favorito>> GetByUserIdAsync(int idUsuario);
        Task<bool> ExistsAsync(int idUsuario, int idEntidad, TipoFavorito tipoEntidad);

        // U - UPDATE (No se requiere en esta tabla, solo se crea o elimina)
        // D - DELETE
        void Remove(Favorito favorito);
    }
}