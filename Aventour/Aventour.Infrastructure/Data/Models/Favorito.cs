using System;
using System.Collections.Generic;

namespace Aventour.Infrastructure.Data.Models;

public partial class Favorito
{
    public int IdUsuario { get; set; }

    public int IdEntidad { get; set; }

    public DateTime? FechaGuardado { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
