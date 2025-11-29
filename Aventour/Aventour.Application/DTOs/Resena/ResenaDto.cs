namespace Aventour.Application.DTOs
{
    public class ResenaDto
    {
        public int IdResena { get; set; }
        public int IdUsuario { get; set; } // Para saber quién la hizo
        public string NombreUsuario { get; set; } // Opcional: útil para mostrar en la UI "Juan P."
        public int IdEntidad { get; set; }
        public string TipoEntidad { get; set; }
        public int Puntuacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}