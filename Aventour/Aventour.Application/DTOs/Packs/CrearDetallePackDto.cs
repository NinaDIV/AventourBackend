using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Packs;

// DTO para los destinos dentro del pack
public class CrearDetallePackDto
{
    [Required]
    public int IdDestino { get; set; }

    [Required]
    public int OrdenParada { get; set; }

    public string? NotasDia { get; set; }
}