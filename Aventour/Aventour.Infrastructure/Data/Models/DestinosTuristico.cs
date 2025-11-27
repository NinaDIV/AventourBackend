using System;
using System.Collections.Generic;

namespace Aventour.Infrastructure.Data.Models;

public partial class DestinosTuristico
{
    public int IdDestino { get; set; }

    public string Nombre { get; set; } = null!;

    public string DescripcionBreve { get; set; } = null!;

    public string? DescripcionCompleta { get; set; }

    public string? Tipo { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public string? HorarioAtencion { get; set; }

    public decimal? CostoEntrada { get; set; }

    public string? UrlFotoPrincipal { get; set; }

    public decimal? PuntuacionMedia { get; set; }

    public virtual ICollection<DetallePackDestino> DetallePackDestinos { get; set; } = new List<DetallePackDestino>();

    public virtual ICollection<DetalleRuta> DetalleRuta { get; set; } = new List<DetalleRuta>();

    public virtual ICollection<PacksRutasAgencium> IdPacks { get; set; } = new List<PacksRutasAgencium>();
}
