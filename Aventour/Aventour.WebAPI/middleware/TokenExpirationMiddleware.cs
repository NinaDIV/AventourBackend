using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Aventour.WebAPI.Middleware
{
    public class TokenExpirationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verificar si hay un token en la cabecera Authorization
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var tokenStr = authHeader.Substring("Bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    var jwtToken = tokenHandler.ReadJwtToken(tokenStr);

                    // Comparar expiración con UTC Now
                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new { message = "Token expirado." });
                        return; // Detener pipeline
                    }
                }
                catch (SecurityTokenException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { message = "Token inválido." });
                    return;
                }
            }

            // Continuar con la pipeline
            await _next(context);
        }
    }
}