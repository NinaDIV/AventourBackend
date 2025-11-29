using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Rutas;

// DTO para actualizar nombre o visibilidad
public class UpdateRutaDto
{
    [Required]
    public string NombreRuta { get; set; } = null!;
    public bool IsPublica { get; set; }
}