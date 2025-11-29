namespace Aventour.Application.DTOs.Rutas;

// DTO de Respuesta: Detalles completos de la ruta
public class RutaResponseDto
{
    public int IdRuta { get; set; }
    public int IdUsuario { get; set; }
    public string NombreRuta { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
    public bool IsPublica { get; set; }
    public List<DetalleRutaResponseDto> Detalles { get; set; } = new();
}

 