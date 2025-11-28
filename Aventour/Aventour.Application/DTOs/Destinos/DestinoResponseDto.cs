namespace Aventour.Application.DTOs.Destinos
{
    public class DestinoResponseDto
    {
        public int IdDestino { get; set; }
        public string Nombre { get; set; } = null!;
        public string DescripcionBreve { get; set; } = null!;
        public string? Tipo { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public decimal? CostoEntrada { get; set; }
        public string? UrlFotoPrincipal { get; set; }
        public decimal? PuntuacionMedia { get; set; }
    }
}