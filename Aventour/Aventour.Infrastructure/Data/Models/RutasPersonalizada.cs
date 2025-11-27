using System;
using System.Collections.Generic;

namespace Aventour.Infrastructure.Data.Models;

public partial class RutasPersonalizada
{
    public int IdRuta { get; set; }

    public int IdUsuario { get; set; }

    public string NombreRuta { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public bool? IsPublica { get; set; }

    public virtual ICollection<DetalleRuta> DetalleRuta { get; set; } = new List<DetalleRuta>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
