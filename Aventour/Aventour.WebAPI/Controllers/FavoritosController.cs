using Aventour.Application.DTOs;
using Aventour.Application.Services.Favoritos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Aventour.Application.DTOs.Favoritos;

namespace Aventour.WebAPI.Controllers
{
    // ADAPTADOR PRIMARIO: Expone el caso de uso GestorFavoritosService
    [Route("api/v1/[controller]")]
    [ApiController]
    // [Authorize] // Asumimos que esta acción está protegida
    public class FavoritosController : ControllerBase
    {
        private readonly GestorFavoritosService _gestorFavoritos;
        
        // Constructor con inyección de dependencia del Caso de Uso
        public FavoritosController(GestorFavoritosService gestorFavoritos)
        {
            _gestorFavoritos = gestorFavoritos;
        }

        private int GetUserIdFromToken()
        {
            // SIMULACIÓN: En una aplicación real, esto se obtiene del claim JWT
            // return int.Parse(User.FindFirst("id_usuario").Value);
            return 1; // ID de usuario de prueba
        }

        /// <summary>
        /// Obtiene todos los favoritos del usuario autenticado.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FavoritoDTO>), 200)]
        public async Task<ActionResult<IEnumerable<FavoritoDTO>>> GetFavoritosUsuario()
        {
            int userId = GetUserIdFromToken();
            var favoritos = await _gestorFavoritos.ObtenerFavoritosPorUsuarioAsync(userId);
            return Ok(favoritos);
        }

        /// <summary>
        /// Agrega un nuevo destino o lugar a la lista de favoritos del usuario.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FavoritoDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)] // Conflict (si ya existe)
        public async Task<ActionResult<FavoritoDTO>> PostFavorito([FromBody] FavoritoInputDTO dto)
        {
            int userId = GetUserIdFromToken();
            
            try
            {
                var nuevoFavorito = await _gestorFavoritos.AgregarFavoritoAsync(userId, dto);
                // Retorna 201 Created
                return CreatedAtAction(nameof(GetFavoritosUsuario), nuevoFavorito); 
            }
            catch (InvalidOperationException ex)
            {
                // Conflict 409: El favorito ya existe
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Bad Request 400: Error de validación o datos
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un favorito de la lista del usuario.
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteFavorito([FromBody] FavoritoInputDTO dto)
        {
            int userId = GetUserIdFromToken();

            try
            {
                // El servicio maneja el caso de que el favorito no exista (retorna true)
                await _gestorFavoritos.EliminarFavoritoAsync(userId, dto);
                
                // Retorna 204 No Content para una eliminación exitosa o si no se encontró
                return NoContent(); 
            }
             catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}