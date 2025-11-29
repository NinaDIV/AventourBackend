using Aventour.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs
{
    public class AgenciaCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = null!;

        [Required]
        public TipoAgenciaGuia Tipo { get; set; } // 'Agencia' o 'Guia'

        [Required]
        [MaxLength(20)]
        public string WhatsappContacto { get; set; } = null!;

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        public string? Descripcion { get; set; }
    }
}