namespace Aventour.Application.DTOs.Rutas;

public class DetalleRutaResponseDto
{
    public int IdDetalle { get; set; }
    public int IdDestino { get; set; }
    public string NombreDestino { get; set; } = null!; // Para mostrar el nombre sin consultar otra API
    public string? UrlFoto { get; set; }
    public int OrdenParada { get; set; }
    public string? Notas { get; set; }
}