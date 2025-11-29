using Aventour.Domain.Entities;

namespace Aventour.Domain.Interfaces
{
    public interface IRutaPersonalizadaRepository
    {
        // Obtener una ruta por ID incluyendo sus detalles
        Task<RutasPersonalizada?> GetByIdAsync(int idRuta);
        
        // Listar rutas de un usuario específico
        Task<IEnumerable<RutasPersonalizada>> GetByUserIdAsync(int idUsuario);
        
        // Crear nueva ruta
        Task<RutasPersonalizada> AddAsync(RutasPersonalizada ruta);
        
        // Actualizar ruta existente
        Task UpdateAsync(RutasPersonalizada ruta);
        
        // Eliminar ruta
        Task DeleteAsync(RutasPersonalizada ruta);
        
        // Gestión de detalles (para agregar destinos a la ruta)
        Task AddDetallesAsync(IEnumerable<DetalleRuta> detalles);
        Task RemoveDetallesAsync(int idRuta); // Para limpiar detalles antes de actualizar si es necesario
    }
}