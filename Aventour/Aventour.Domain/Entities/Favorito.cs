using Aventour.Domain.Entities;
using Aventour.Domain.Enums;

namespace Aventour.Domain.Models;

public partial class Favorito
{
    public int IdUsuario { get; set; }

    public int IdEntidad { get; set; }
    
    // Ahora usa el enum
    public TipoFavorito TipoEntidad { get; set; }
    public DateTime? FechaGuardado { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}