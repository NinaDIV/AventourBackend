namespace Aventour.Domain.Entities;

public partial class RutasPersonalizada
{
    public int IdRuta { get; set; }

    public int IdUsuario { get; set; }

    public string NombreRuta { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public bool? IsPublica { get; set; }

    // Propiedades de Navegación
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    public virtual ICollection<DetalleRuta> DetalleRuta { get; set; } = new List<DetalleRuta>();
}
