namespace Aventour.Application.DTOs.Usuarios;

// DTO de respuesta (sin password)
public class UsuarioResponseDto
{
    public int IdUsuario { get; set; }
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EsAdministrador { get; set; }
    public string? Token { get; set; } // Opcional: Para devolver el JWT al hacer login
    
    // Devolvemos el Rol en lugar del booleano para que el Front sepa quién es
    public string Rol { get; set; }
    
}