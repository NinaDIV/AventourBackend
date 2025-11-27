using Aventour.Application.DTOs.Usuarios;
using Aventour.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // ============================================================
        // 1. REGISTRO
        // ============================================================
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] CrearUsuarioDto dto)
        {
            try
            {
                var result = await _usuarioService.RegistrarUsuarioAsync(dto);

                return Ok(new
                {
                    mensaje = "Usuario registrado correctamente.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // ============================================================
        // 2. LOGIN (Genera token JWT)
        // ============================================================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _usuarioService.LoginAsync(dto);

                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        // ============================================================
        // 3. OBTENER POR ID
        // * Puedes activar autorización con roles cuando quieras *
        // ============================================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var result = await _usuarioService.ObtenerPorIdAsync(id);

            if (result == null)
                return NotFound(new { error = "Usuario no encontrado." });

            return Ok(new
            {
                mensaje = "Usuario encontrado.",
                data = result
            });
        }

        // ============================================================
        // 4. ACTUALIZAR USUARIO
        // ============================================================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateUsuarioDto dto)
        {
            try
            {
                await _usuarioService.ActualizarUsuarioAsync(id, dto);

                return Ok(new
                {
                    mensaje = "Usuario actualizado correctamente."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
