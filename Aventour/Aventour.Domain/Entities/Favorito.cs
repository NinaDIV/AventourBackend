using Aventour.Domain.Entities;

namespace Aventour.Domain.Models;

public partial class Favorito
{
    public int IdUsuario { get; set; }

    public int IdEntidad { get; set; }
    
    public string TipoEntidad { get; set; } = null!;

    public DateTime? FechaGuardado { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}