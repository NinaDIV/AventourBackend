using Aventour.Application.DTOs.Destinos;

namespace Aventour.Application.Repositories;

public interface IDestinoService
{
    Task<IEnumerable<DestinoResponseDto>> ListarTodos();
    Task<DestinoResponseDto> ObtenerPorId(int id);
    Task<int> Crear(CrearDestinoDto dto);
    Task Actualizar(UpdateDestinoDto dto);
        
    Task ActualizarPuntuacionMedia(int id);

        
        
 

    Task Eliminar(int id);
}