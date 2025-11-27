namespace Aventour.Application.DTOs.Auth;

public record AuthResponseDto(
    int Id,
    string Email,
    string Nombre,
    string Rol,
    string Token
);