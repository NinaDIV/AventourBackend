namespace Aventour.Domain.Entities;

public partial class PacksRutasAgencium
{
    public int IdPack { get; set; }

    public int IdAgencia { get; set; }

    public string NombrePack { get; set; } = null!;

    public decimal PrecioBase { get; set; }

    public string? Descripcion { get; set; }

    public int? DuracionDias { get; set; }

    public virtual ICollection<DetallePackDestino> DetallePackDestinos { get; set; } = new List<DetallePackDestino>();

    public virtual AgenciasGuia IdAgenciaNavigation { get; set; } = null!;

    public virtual ICollection<DestinosTuristico> IdDestinos { get; set; } = new List<DestinosTuristico>();
}
