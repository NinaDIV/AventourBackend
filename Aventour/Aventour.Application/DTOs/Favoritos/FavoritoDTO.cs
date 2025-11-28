using Aventour.Domain.Entities;
using Aventour.Domain.Enums;

namespace Aventour.Application.DTOs
{
    public class FavoritoDTO
    {
        public int IdUsuario { get; set; }
        public int IdEntidad { get; set; }
        public string TipoEntidad { get; set; }
        public DateTime FechaGuardado { get; set; }

        public static FavoritoDTO FromEntity(Favorito favorito)
        {
            return new FavoritoDTO
            {
                IdUsuario = favorito.IdUsuario,
                IdEntidad = favorito.IdEntidad,
                TipoEntidad = favorito.TipoEntidad.ToString(),
                FechaGuardado = favorito.FechaGuardado
            };
        }
    }
}