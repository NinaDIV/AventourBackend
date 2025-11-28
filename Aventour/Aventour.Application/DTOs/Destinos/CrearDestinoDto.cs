namespace Aventour.Application.DTOs.Destinos
{
    public class CrearDestinoDto
    {
        public string Nombre { get; set; } = null!;
        public string DescripcionBreve { get; set; } = null!;
        public string? DescripcionCompleta { get; set; }
        public string? Tipo { get; set; } // Playa, Montaña, Historico, etc.
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? HorarioAtencion { get; set; }
        public decimal? CostoEntrada { get; set; }
        public string? UrlFotoPrincipal { get; set; }
    }
}