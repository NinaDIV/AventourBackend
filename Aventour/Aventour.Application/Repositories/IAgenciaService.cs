using Aventour.Application.DTOs;
using Aventour.Domain.Enums;

namespace Aventour.Application.Services.Agencias
{
    public interface IAgenciaService
    {
        Task<List<AgenciaDto>> GetAllAsync(TipoAgenciaGuia? tipo);
        Task<AgenciaDto> GetByIdAsync(int id);
        Task<AgenciaDto> CreateAsync(AgenciaCreateDto dto);
        Task UpdateAsync(int id, AgenciaUpdateDto dto);
        Task DeleteAsync(int id);
        
        // Métodos especiales
        Task ValidarAgenciaAsync(int id, bool estadoValidacion); // Solo admins
        Task ActualizarPuntuacionAsync(int id); // Se llama cuando se crea una reseña
    }
}