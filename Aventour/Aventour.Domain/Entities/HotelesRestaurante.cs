namespace Aventour.Domain.Entities;

public partial class HotelesRestaurante
{
    public int IdLugar { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public string? Direccion { get; set; }

    public decimal? PuntuacionMedia { get; set; }
}
