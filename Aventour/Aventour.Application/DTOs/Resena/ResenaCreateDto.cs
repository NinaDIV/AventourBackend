using Aventour.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs
{
    public class ResenaCreateDto
    {
        [Required]
        public int IdEntidad { get; set; }

        [Required]
        public TipoResena TipoEntidad { get; set; } // Destino, Agencia, Guia

        [Required]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
        public int Puntuacion { get; set; }

        [MaxLength(1000)]
        public string? Comentario { get; set; }
    }
}