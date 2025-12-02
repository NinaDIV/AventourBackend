using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aventour.WebAPI.Controllers.Base
{
    /// <summary>
    /// Controlador base para endpoints que requieren autenticación.
    /// Proporciona métodos auxiliares para extraer información del usuario autenticado.
    /// </summary>
    [ApiController]
    public abstract class BaseAuthenticatedController : ControllerBase
    {
        /// <summary>
        /// Obtiene el ID del usuario autenticado desde los claims del token JWT.
        /// Intenta múltiples claims en orden de prioridad.
        /// </summary>
        /// <returns>ID del usuario autenticado</returns>
        /// <exception cref="UnauthorizedAccessException">Si no se puede identificar al usuario</exception>
        protected int ObtenerIdUsuarioAutenticado()
        {
            // 1. Intentar con claim "id" (configurado en JwtTokenGenerator)
            var idClaim = User.FindFirst("id")?.Value;

            // 2. Fallback: Intentar con "id_usuario"
            if (string.IsNullOrEmpty(idClaim))
                idClaim = User.FindFirst("id_usuario")?.Value;

            // 3. Fallback: Intentar con NameIdentifier (estándar .NET)
            if (string.IsNullOrEmpty(idClaim))
                idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Validar y parsear el ID
            if (int.TryParse(idClaim, out int userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("No se pudo identificar al usuario desde el token JWT.");
        }

        /// <summary>
        /// Obtiene el email del usuario autenticado desde los claims del token JWT.
        /// </summary>
        /// <returns>Email del usuario autenticado</returns>
        /// <exception cref="UnauthorizedAccessException">Si no se puede identificar el email</exception>
        protected string ObtenerEmailUsuarioAutenticado()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                // 1. Intentar con NameIdentifier (podría ser el email)
                var email = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // 2. Fallback: Intentar con claim "sub"
                if (string.IsNullOrEmpty(email))
                    email = identity.FindFirst("sub")?.Value;

                // 3. Fallback: Intentar con claim Email estándar
                if (string.IsNullOrEmpty(email))
                    email = identity.FindFirst(ClaimTypes.Email)?.Value;

                // 4. Fallback: Intentar con claim "email" minúsculas
                if (string.IsNullOrEmpty(email))
                    email = identity.FindFirst("email")?.Value;

                if (!string.IsNullOrEmpty(email))
                    return email;
            }

            throw new UnauthorizedAccessException("No se pudo identificar el email del usuario en el token JWT.");
        }

        /// <summary>
        /// Obtiene el rol del usuario autenticado desde los claims del token JWT.
        /// </summary>
        /// <returns>Rol del usuario o null si no se encuentra</returns>
        protected string? ObtenerRolUsuarioAutenticado()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value 
                   ?? User.FindFirst("role")?.Value;
        }

        /// <summary>
        /// Verifica si el usuario autenticado tiene un rol específico.
        /// </summary>
        /// <param name="rol">Rol a verificar</param>
        /// <returns>True si el usuario tiene el rol, False en caso contrario</returns>
        protected bool UsuarioTieneRol(string rol)
        {
            var userRole = ObtenerRolUsuarioAutenticado();
            return !string.IsNullOrEmpty(userRole) && 
                   userRole.Equals(rol, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Crea una respuesta de éxito estándar.
        /// </summary>
        protected IActionResult SuccessResponse(object data, string mensaje = "Operación exitosa")
        {
            return Ok(new
            {
                success = true,
                mensaje,
                data
            });
        }

        /// <summary>
        /// Crea una respuesta de error estándar.
        /// </summary>
        protected IActionResult ErrorResponse(string mensaje, int statusCode = 400)
        {
            return StatusCode(statusCode, new
            {
                success = false,
                error = mensaje
            });
        }
    }
}

