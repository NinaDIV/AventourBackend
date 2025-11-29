using Aventour.Domain.Enums;

namespace Aventour.Domain.Entities;

public partial class AgenciasGuia
{
    public int IdAgencia { get; set; }

    public string Nombre { get; set; } = null!;
    
    public TipoAgenciaGuia Tipo { get; set; }

    public string WhatsappContacto { get; set; } = null!;

    public string? Email { get; set; }

    public string? Descripcion { get; set; }

    public bool? Validado { get; set; }

    public decimal? PuntuacionMedia { get; set; }

    public virtual ICollection<PacksRutasAgencium> PacksRutasAgencia { get; set; } = new List<PacksRutasAgencium>();
}
