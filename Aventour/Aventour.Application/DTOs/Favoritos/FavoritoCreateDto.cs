using Aventour.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs
{
    public class FavoritoCreateDto
    {
        // El IdUsuario se obtendrá del token o la sesión
        public int IdEntidad { get; set; }
        public TipoFavorito TipoEntidad { get; set; }
    }
}