using Aventour.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs
{
    public class FavoritoCreateDto
    {
        [Required]
        public int IdEntidad { get; set; } // ID del Destino o Lugar

        [Required]
        public TipoFavorito TipoEntidad { get; set; } // 'Destino' o 'Lugar'
    }
}