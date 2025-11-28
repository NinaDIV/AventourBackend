using Aventour.Application.DTOs;
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Aventour.Application.Services.Favoritos;
using Aventour.Domain.Enums;

namespace Aventour.WebAPI.Controllers
{
    
    
    
    
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize] // <--- ¡IMPORTANTE! Solo usuarios logueados pueden entrar aquí
    public class FavoritosController : ControllerBase
    {
        
        
        private readonly IFavoritoService _favoritoService;

        public FavoritosController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        private int ObtenerIdUsuarioAutenticado()
        {
            // Intenta primero el claim "id" (como pusiste en tu JWT)
            var idClaim = User.FindFirst("id")?.Value;

            // Fallback a "id_usuario" si cambió
            if (string.IsNullOrEmpty(idClaim))
                idClaim = User.FindFirst("id_usuario")?.Value;

            // Fallback al estándar NameIdentifier
            if (string.IsNullOrEmpty(idClaim))
                idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(idClaim, out int id))
                return id;

            throw new UnauthorizedAccessException("No se pudo identificar al usuario desde el token.");
        }


        [HttpGet]
        public async Task<ActionResult<List<FavoritoDto>>> GetMisFavoritos()
        {
            int userId = ObtenerIdUsuarioAutenticado(); // ID automático
            var favoritos = await _favoritoService.GetFavoritosDelUsuarioAsync(userId);
            return Ok(favoritos);
        }

        [HttpPost]
        public async Task<ActionResult<FavoritoDto>> AgregarFavorito([FromBody] FavoritoCreateDto dto)
        {
            try
            {
                int userId = ObtenerIdUsuarioAutenticado(); // ID automático
                var resultado = await _favoritoService.AddFavoritoAsync(userId, dto);
                return CreatedAtAction(nameof(GetMisFavoritos), resultado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // 409 si ya existe
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarFavorito([FromBody] FavoritoCreateDto dto)
        {
            int userId = ObtenerIdUsuarioAutenticado(); 
            
            bool eliminado = await _favoritoService.RemoveFavoritoAsync(userId, dto.IdEntidad, dto.TipoEntidad);
            
            if (!eliminado) return NotFound(new { message = "El favorito no existe." });
            
            return NoContent();
        }
        
        // GET: api/v1/Favoritos/Destino/1
        [HttpGet("{tipoEntidad}/{idEntidad}")]
        public async Task<ActionResult<FavoritoDto>> ObtenerFavorito(TipoFavorito tipoEntidad, int idEntidad)
        {
            // Extraemos el ID del usuario del Token JWT
            var idUsuarioStr = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(idUsuarioStr)) return Unauthorized("Token inválido");
            int idUsuario = int.Parse(idUsuarioStr);

            try
            {
                var favorito = await _favoritoService.GetFavoritoByIdAsync(idUsuario, idEntidad, tipoEntidad);
                return Ok(favorito);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

// PUT: api/v1/Favoritos/Destino/1
        [HttpPut]
        public async Task<IActionResult> ActualizarFavorito([FromBody] FavoritoCreateDto dto)
        {
            try
            {
                // 1. Obtener ID del usuario desde el Token
                int userId = ObtenerIdUsuarioAutenticado();

                // 2. Llamar al servicio usando los datos del JSON
                await _favoritoService.UpdateFavoritoAsync(userId, dto.IdEntidad, dto.TipoEntidad);
        
                // 3. Retornar éxito
                return NoContent(); // 204 OK
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}