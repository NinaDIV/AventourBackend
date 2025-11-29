using Aventour.Domain.Entities;
using Aventour.Domain.Models;
using Aventour.Domain.Enums;

namespace Aventour.Domain.Interfaces
{
    public interface IResenaRepository
    {
        // Obtener reseñas de una entidad (ej. Destino X)
        Task<List<Resena>> GetResenasByEntidadAsync(int idEntidad, TipoResena tipoEntidad);
        
        // Obtener reseñas hechas por un usuario específico
        Task<List<Resena>> GetResenasByUsuarioAsync(int idUsuario);
        
        // Buscar una reseña específica (para validar duplicados)
        Task<Resena?> GetResenaEspecificaAsync(int idUsuario, int idEntidad, TipoResena tipoEntidad);
        
        // Obtener por ID (para eliminar)
        Task<Resena?> GetByIdAsync(int idResena);

        Task AddResenaAsync(Resena resena);
        Task DeleteResenaAsync(Resena resena);
    }
}