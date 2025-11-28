using Aventour.Domain.Enums;

namespace Aventour.Application.DTOs.Favoritos;

public class FavoritoResponseDto
{
    public int IdEntidad { get; set; }
    public TipoFavorito TipoEntidad { get; set; }
    public DateTime FechaGuardado { get; set; }
    
    // Opcional: Podrías incluir un campo "NombreEntidad" si haces un join manual en el servicio.
}