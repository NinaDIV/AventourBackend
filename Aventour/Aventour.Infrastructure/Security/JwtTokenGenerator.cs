using Aventour.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Aventour.Domain.Enums;

namespace Aventour.Infrastructure.Authentication
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            // 1. Determinar el rol basado en tu booleano de la DB

            var rol = (usuario.EsAdministrador == true) ? RolUsuario.Admin : RolUsuario.Turista;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", usuario.IdUsuario.ToString()),
                new Claim("nombre", usuario.Nombres),
                new Claim("isAdmin", usuario.EsAdministrador.GetValueOrDefault().ToString()),
                // 2. AGREGAR EL ROL COMO CLAIM ESTÁNDAR
                // Esto permite usar [Authorize(Roles = "Admin")] en los controladores
                new Claim(ClaimTypes.Role, rol.ToString())
            };
 
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8), // El token dura 2 horas
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}