using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Rutas;
public class CrearRutaDto
{
    [Required(ErrorMessage = "El nombre de la ruta es obligatorio")]
    public string NombreRuta { get; set; } = null!;
        
    public bool IsPublica { get; set; } = false;

    // Opcional: Permitir agregar destinos al crear la ruta
    public List<CrearDetalleRutaDto>? Destinos { get; set; }
}