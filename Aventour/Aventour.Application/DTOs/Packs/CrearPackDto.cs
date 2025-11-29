using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Packs; 
public class CrearPackDto
{
    [Required(ErrorMessage = "El nombre del pack es obligatorio")]
    public string NombrePack { get; set; } = null!;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PrecioBase { get; set; }

    public string? Descripcion { get; set; }

    [Range(1, 365)]
    public int DuracionDias { get; set; } = 1;

    // Lista de destinos incluidos en el pack
    public List<CrearDetallePackDto> Destinos { get; set; } = new();
}