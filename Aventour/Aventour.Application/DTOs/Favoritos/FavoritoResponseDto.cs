namespace Aventour.Application.DTOs.Favoritos
{
    public class FavoritoResponseDto
    {
        public int IdDestino { get; set; }
        public string NombreDestino { get; set; } = null!;
        public string DescripcionBreve { get; set; } = null!;
        public string? UrlFotoPrincipal { get; set; }
        public DateTime? FechaGuardado { get; set; }
    }
}