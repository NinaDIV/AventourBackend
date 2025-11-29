using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Lugares;

public class CrearLugarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = null!;

    [Required]
    // Se espera "Hotel" o "Restaurante"
    public string Tipo { get; set; } = null!; 

    [Required]
    public decimal Latitud { get; set; }

    [Required]
    public decimal Longitud { get; set; }

    public string? Direccion { get; set; }
}