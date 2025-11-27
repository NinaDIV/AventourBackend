using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Auth;

public record RegistroUsuarioDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellidos { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; init; } = string.Empty;

    [Range(18, 120)]
    public int Edad { get; init; }

    // Opcional: Permitir elegir rol en registro o forzarlo en el servicio
    public string Rol { get; init; } = "Turista"; 
}