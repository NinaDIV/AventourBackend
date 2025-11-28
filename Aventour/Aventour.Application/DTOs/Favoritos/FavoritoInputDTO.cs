using System.ComponentModel.DataAnnotations;
using Aventour.Domain.Enums;

namespace Aventour.Application.DTOs.Favoritos;

public class FavoritoInputDTO
{
    // El IdUsuario se obtendría del JWT/Claim en la capa WebAPI, no del cuerpo de la solicitud, 
    // pero se incluye aquí para la estructura de la aplicación.
    // [Required]
    // public int IdUsuario { get; set; } 
        
    [Required(ErrorMessage = "El identificador de la entidad es obligatorio.")]
    public int IdEntidad { get; set; }
        
    [Required(ErrorMessage = "El tipo de entidad (Destino o Lugar) es obligatorio.")]
    public TipoFavorito TipoEntidad { get; set; }
}