using Aventour.Application.DTOs.Rutas;
using Aventour.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aventour.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class RutaPersonalizadaController : ControllerBase
    {
        private readonly RutaPersonalizadaService _rutaService;

        public RutaPersonalizadaController(RutaPersonalizadaService rutaService)
        {
            _rutaService = rutaService;
        }

        // --- MÉTODO AUXILIAR PARA OBTENER ID ROBUSTO ---
        private int ObtenerIdUsuarioAutenticado()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                // 1. Prioridad: Buscar el claim "id" que pusimos en el JwtTokenGenerator
                var idClaim = identity.FindFirst("id");
                if (idClaim != null && int.TryParse(idClaim.Value, out int id))
                {
                    return id;
                }

                // 2. Fallback: Buscar NameIdentifier (estándar de .NET)
                var nameIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                if (nameIdClaim != null && int.TryParse(nameIdClaim.Value, out int idFromSub))
                {
                    return idFromSub;
                }
            }
            
            throw new UnauthorizedAccessException("ID de usuario no encontrado en el Token.");
        }
        // -----------------------------------------------

        [HttpPost]
        public async Task<IActionResult> CrearRuta([FromBody] CrearRutaDto dto)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioAutenticado();
                var idRuta = await _rutaService.CrearRutaAsync(idUsuario, dto);
                return Ok(new { message = "Ruta creada con éxito", idRuta = idRuta });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMisRutas()
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioAutenticado();
                var rutas = await _rutaService.ObtenerRutasPorUsuarioAsync(idUsuario);
                return Ok(rutas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerDetalleRuta(int id)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioAutenticado();
                var ruta = await _rutaService.ObtenerDetalleRutaAsync(id, idUsuario);
                return Ok(ruta);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ruta no encontrada." });
            }
            catch (UnauthorizedAccessException ex)
            {
                // CORRECCIÓN: Usar StatusCode 403 en lugar de Forbid(string)
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarRuta(int id, [FromBody] UpdateRutaDto dto)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioAutenticado();
                await _rutaService.EditarRutaAsync(id, idUsuario, dto);
                return NoContent(); 
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ruta no encontrada." });
            }
            catch (UnauthorizedAccessException ex)
            {
                // CORRECCIÓN: Usar StatusCode 403 para evitar el error 500 "No authentication handler..."
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarRuta(int id)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioAutenticado();
                await _rutaService.EliminarRutaAsync(id, idUsuario);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ruta no encontrada." });
            }
            catch (UnauthorizedAccessException ex)
            {
                // CORRECCIÓN: Usar StatusCode 403
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("{id}/destinos")]
        public async Task<IActionResult> AgregarDestinos(int id, [FromBody] List<CrearDetalleRutaDto> destinos)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioAutenticado();
                await _rutaService.AgregarDestinosARutaAsync(id, idUsuario, destinos);
                return Ok(new { message = "Destinos agregados correctamente." });
            }
             catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}