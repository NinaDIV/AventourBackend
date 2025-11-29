using Aventour.Domain.Entities;

 
 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aventour.Domain.Interfaces
{
    public interface IHotelRestauranteRepository
    {
        Task<HotelesRestaurante?> GetByIdAsync(int id);
        Task<IEnumerable<HotelesRestaurante>> GetAllAsync(string? tipo = null);
        Task<HotelesRestaurante> AddAsync(HotelesRestaurante entidad);
        Task UpdateAsync(HotelesRestaurante entidad);
        Task DeleteAsync(HotelesRestaurante entidad);
    }
}
