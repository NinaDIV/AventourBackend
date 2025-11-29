namespace Aventour.Domain.Interfaces;

using Aventour.Domain.Entities; // o Models según tu estructura

public interface IAgenciaRepository
{
    Task<AgenciasGuia> GetByIdAsync(int id);
    // Otros métodos que necesites (Agregar, Eliminar, etc)
}
