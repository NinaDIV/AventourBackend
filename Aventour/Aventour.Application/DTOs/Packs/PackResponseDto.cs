namespace Aventour.Application.DTOs.Packs;

public class PackResponseDto
{
    public int IdPack { get; set; }
    public int IdAgencia { get; set; }
    public string NombreAgencia { get; set; } = string.Empty; // Extra info útil
    public string NombrePack { get; set; } = null!;
    public decimal PrecioBase { get; set; }
    public string? Descripcion { get; set; }
    public int? DuracionDias { get; set; }
        
    public List<DetallePackResponseDto> Destinos { get; set; } = new();
}