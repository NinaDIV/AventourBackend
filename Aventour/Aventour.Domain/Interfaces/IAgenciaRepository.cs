using Aventour.Domain.Entities;
using Aventour.Domain.Enums;

namespace Aventour.Domain.Interfaces
{
    public interface IAgenciaRepository
    {
        Task<List<AgenciasGuia>> GetAllAsync(TipoAgenciaGuia? filtroTipo); // Opción para filtrar solo guías o agencias
        Task<AgenciasGuia?> GetByIdAsync(int id);
        Task AddAsync(AgenciasGuia entidad);
        Task UpdateAsync(AgenciasGuia entidad); // Generic update (EF Core tracking)
        Task DeleteAsync(AgenciasGuia entidad);
        
        // Método especial para recalcular el promedio desde la tabla Reseñas
        Task RecalcularPuntuacionMediaAsync(int idAgencia);
    }
}