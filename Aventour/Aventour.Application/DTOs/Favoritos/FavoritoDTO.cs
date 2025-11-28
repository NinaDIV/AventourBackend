using Aventour.Domain.Enums;

namespace Aventour.Application.DTOs
{
    public class FavoritoDto
    {
        public int IdUsuario { get; set; }
        public int IdEntidad { get; set; }
        public string TipoEntidad { get; set; } // Lo devolvemos como string para lectura fácil
        public DateTime FechaGuardado { get; set; }
    }
}