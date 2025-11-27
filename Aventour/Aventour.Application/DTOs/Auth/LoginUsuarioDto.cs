using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Auth;

public record LoginUsuarioDto(
    [Required] [EmailAddress] string Email,
    [Required] string Password
);