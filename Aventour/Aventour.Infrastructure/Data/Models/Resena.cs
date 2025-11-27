using System;
using System.Collections.Generic;

namespace Aventour.Infrastructure.Data.Models;

public partial class Resena
{
    public int IdResena { get; set; }

    public int IdUsuario { get; set; }

    public int IdEntidad { get; set; }

    public int Puntuacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
