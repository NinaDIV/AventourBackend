namespace Aventour.Application.DTOs.Usuarios;

// DTO para actualizar
public class UpdateUsuarioDto
{
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public int? Edad { get; set; }
    public string? EstadoCivil { get; set; }
}