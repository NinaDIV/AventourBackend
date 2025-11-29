namespace Aventour.Application.DTOs.Packs;

public class DetallePackResponseDto
{
    public int IdDetallePack { get; set; }
    public int IdDestino { get; set; }
    public string NombreDestino { get; set; } = string.Empty;
    public string? UrlFoto { get; set; }
    public int OrdenParada { get; set; }
    public string? NotasDia { get; set; }
}