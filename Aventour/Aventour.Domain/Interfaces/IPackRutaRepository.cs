using Aventour.Domain.Entities;

namespace Aventour.Domain.Interfaces
{
    public interface IPackRutaRepository
    {
        // Obtener pack por ID con detalles
        Task<PacksRutasAgencium?> GetByIdAsync(int idPack);

        // Listar todos los packs (Público)
        Task<IEnumerable<PacksRutasAgencium>> GetAllAsync();

        // Crear pack
        Task<PacksRutasAgencium> AddAsync(PacksRutasAgencium pack);

        // Actualizar pack
        Task UpdateAsync(PacksRutasAgencium pack);

        // Eliminar pack
        Task DeleteAsync(PacksRutasAgencium pack);

        // Métodos auxiliares para manejo de detalles
        Task AddDetallesAsync(IEnumerable<DetallePackDestino> detalles);
        Task RemoveDetallesAsync(int idPack);

        // Validación de Agencia: Buscar ID de agencia por Email del usuario logueado
        Task<int?> GetAgenciaIdByEmailAsync(string email);
    }
}