using Aventour.Domain.Entities;
using Aventour.Domain.Enums;

namespace Aventour.Application.DTOs
{
    public class FavoritoDto
    {
        public int IdUsuario { get; set; }
        public int IdEntidad { get; set; }
        public TipoFavorito TipoEntidad { get; set; }
        public DateTime? FechaGuardado { get; set; }
    }
}