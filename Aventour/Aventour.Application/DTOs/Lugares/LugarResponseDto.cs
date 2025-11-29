namespace Aventour.Application.DTOs.Lugares;


public class LugarResponseDto
{
    public int IdLugar { get; set; }
    public string Nombre { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public string? Direccion { get; set; }
    public decimal? PuntuacionMedia { get; set; }
}