namespace Aventour.Domain.Entities;

public partial class DetalleRuta
{
    public int IdDetalle { get; set; }

    public int IdRuta { get; set; }

    public int IdDestino { get; set; }

    public int OrdenParada { get; set; }

    public string? Notas { get; set; }

    public virtual DestinosTuristico IdDestinoNavigation { get; set; } = null!;

    public virtual RutasPersonalizada IdRutaNavigation { get; set; } = null!;
    
    
}
