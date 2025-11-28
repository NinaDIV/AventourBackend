using Aventour.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs
{
    public class FavoritoCreationDto
    {
        [Required]
        public int IdUsuario { get; set; }
        
        [Required]
        public int IdEntidad { get; set; }

        [Required]
        public TipoFavorito TipoEntidad { get; set; }
    }
}