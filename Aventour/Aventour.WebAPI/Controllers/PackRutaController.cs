using Aventour.Application.DTOs.Packs;
using Aventour.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Aventour.Application.Services.Packs;

namespace Aventour.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackRutaController : ControllerBase
    {
        private readonly PackRutaService _packService;

        public PackRutaController(PackRutaService packService)
        {
            _packService = packService;
        }

        private string ObtenerEmailUsuario()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            // Busca el Claim Sub (Email) o Name
            var email = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? identity?.FindFirst("sub")?.Value 
                     ?? identity?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                throw new UnauthorizedAccessException("No se pudo identificar el email del usuario en el token.");

            return email;
        }

        // GET: api/PackRuta (Público)
        [HttpGet]
        public async Task<IActionResult> ListarPacks()
        {
            try
            {
                var packs = await _packService.ListarTodosAsync();
                return Ok(packs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/PackRuta/{id} (Público)
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerDetalle(int id)
        {
            try
            {
                var pack = await _packService.ObtenerPorIdAsync(id);
                return Ok(pack);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Pack no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/PackRuta (Requiere Auth)
        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> CrearPack([FromBody] CrearPackDto dto)
        {
            try
            {
                string email = ObtenerEmailUsuario();
                var idPack = await _packService.CrearPackAsync(email, dto);
                return CreatedAtAction(nameof(ObtenerDetalle), new { id = idPack }, new { id = idPack, message = "Pack creado exitosamente" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message }); // Retorna 403 si no es agencia
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/PackRuta/{id} (Requiere Auth)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> ActualizarPack(int id, [FromBody] UpdatePackDto dto)
        {
            try
            {
                string email = ObtenerEmailUsuario();
                await _packService.ActualizarPackAsync(id, email, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Pack no encontrado." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/PackRuta/{id} (Requiere Auth)
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> EliminarPack(int id)
        {
            try
            {
                string email = ObtenerEmailUsuario();
                await _packService.EliminarPackAsync(id, email);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Pack no encontrado." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}