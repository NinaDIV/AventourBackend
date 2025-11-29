using Aventour.Application.DTOs;
using Aventour.Application.Services.Resenas;
using Aventour.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aventour.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ResenasController : ControllerBase
    {
        private readonly IResenaService _resenaService;

        public ResenasController(IResenaService resenaService)
        {
            _resenaService = resenaService;
        }

        // Método auxiliar para extraer ID del token (reutilizado de Favoritos)
        private int ObtenerIdUsuarioAutenticado()
        {
            var idClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(idClaim)) idClaim = User.FindFirst("id_usuario")?.Value;
            if (string.IsNullOrEmpty(idClaim)) idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(idClaim, out int id)) return id;
            throw new UnauthorizedAccessException("No se pudo identificar al usuario desde el token.");
        }

        // 1. Crear Reseña
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResenaDto>> CrearResena([FromBody] ResenaCreateDto dto)
        {
            try
            {
                int userId = ObtenerIdUsuarioAutenticado();
                var resultado = await _resenaService.AddResenaAsync(userId, dto);
                
                // Retorna 201 Created
                return CreatedAtAction(nameof(GetMisResenas), new { }, resultado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex) // Para duplicados
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // 2. Listar Reseñas de una Entidad (Público, no requiere Authorize obligatoriamente, pero depende de tu regla)
        // Ejemplo: GET api/v1/Resenas/entidad/Destino/5
        [HttpGet("entidad/{tipoEntidad}/{idEntidad}")]
        public async Task<ActionResult<List<ResenaDto>>> GetResenasEntidad(TipoResena tipoEntidad, int idEntidad)
        {
            var lista = await _resenaService.GetResenasPorEntidadAsync(idEntidad, tipoEntidad);
            return Ok(lista);
        }

        // 3. Listar mis Reseñas
        [HttpGet("usuario")]
        [Authorize]
        public async Task<ActionResult<List<ResenaDto>>> GetMisResenas()
        {
            int userId = ObtenerIdUsuarioAutenticado();
            var lista = await _resenaService.GetResenasDelUsuarioAsync(userId);
            return Ok(lista);
        }

        // 4. Eliminar Reseña
        [HttpDelete("{idResena}")]
        [Authorize]
        public async Task<IActionResult> EliminarResena(int idResena)
        {
            try 
            {
                int userId = ObtenerIdUsuarioAutenticado();
                bool eliminado = await _resenaService.DeleteResenaAsync(userId, idResena);

                if (!eliminado) return NotFound(new { message = "La reseña no existe." });

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // 403 Forbidden si intenta borrar reseña ajena
            }
        }
    }
}