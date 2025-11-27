namespace Aventour.Application.DTOs.Usuarios;

// DTO para Login
public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}