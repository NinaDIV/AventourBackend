using Aventour.Domain.Enums;

namespace Aventour.Application.DTOs.Usuarios;

// DTO para registrarse
public class CrearUsuarioDto
{
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!; // Contraseña plana
    public int? Edad { get; set; }
    // Por defecto será Turista
    public RolUsuario Rol { get; set; } = RolUsuario.Turista;
}