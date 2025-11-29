using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Rutas;

// DTO para agregar un destino a la ruta
public class CrearDetalleRutaDto
{
    [Required]
    public int IdDestino { get; set; }
        
    [Required]
    public int OrdenParada { get; set; }
        
    public string? Notas { get; set; }
}