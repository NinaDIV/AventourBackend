namespace Aventour.Domain.Entities;

public partial class DetallePackDestino
{
    public int IdDetallePack { get; set; }

    public int IdPack { get; set; }

    public int IdDestino { get; set; }

    public int OrdenParada { get; set; }

    public string? NotasDia { get; set; }

    public virtual DestinosTuristico IdDestinoNavigation { get; set; } = null!;

    public virtual PacksRutasAgencium IdPackNavigation { get; set; } = null!;
}
