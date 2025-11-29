namespace Aventour.Application.DTOs
{
    public class AgenciaDto
    {
        public int IdAgencia { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; } // Lo devolvemos como string
        public string WhatsappContacto { get; set; }
        public string? Email { get; set; }
        public string? Descripcion { get; set; }
        public bool? Validado { get; set; }
        public decimal? PuntuacionMedia { get; set; }
    }
}