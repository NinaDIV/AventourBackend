using Aventour.Application.DTOs.Packs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aventour.Application.Services.Packs;
using Aventour.WebAPI.Controllers.Base;

namespace Aventour.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    public class PackRutaController : BaseAuthenticatedController
    {
        private readonly PackRutaService _packService;

        public PackRutaController(PackRutaService packService)
        {
            _packService = packService;
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

        // POST: api/v1/PackRuta (Requiere Auth)
        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> CrearPack([FromBody] CrearPackDto dto)
        {
            try
            {
                string email = ObtenerEmailUsuarioAutenticado();
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

        // PUT: api/v1/PackRuta/{id} (Requiere Auth)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> ActualizarPack(int id, [FromBody] UpdatePackDto dto)
        {
            try
            {
                string email = ObtenerEmailUsuarioAutenticado();
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

        // DELETE: api/v1/PackRuta/{id} (Requiere Auth)
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> EliminarPack(int id)
        {
            try
            {
                string email = ObtenerEmailUsuarioAutenticado();
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