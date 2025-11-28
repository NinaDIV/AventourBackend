// Aventour.WebAPI.Controllers/FavoritosController.cs

using Aventour.Application.DTOs; // Usaremos los DTOs definidos previamente
using Aventour.Application.UseCases.Favoritos; // Usamos el Caso de Uso (el servicio)
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
// using Aventour.Application.DTOs.Favoritos; // Esta referencia ya no es necesaria si usamos Aventour.Application.DTOs

namespace Aventour.WebAPI.Controllers
{
    // ADAPTADOR PRIMARIO: Expone el caso de uso IFavoritoService
    [Route("api/v1/[controller]")]
    [ApiController]
    // [Authorize] // Asumimos que esta acción está protegida
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritoService _favoritoService; // Usamos la interfaz
        
        // Constructor con inyección de dependencia del Caso de Uso
        public FavoritosController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
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
        [ProducesResponseType(typeof(IEnumerable<FavoritoDto>), 200)] // Corregido a FavoritoDto
        public async Task<ActionResult<IEnumerable<FavoritoDto>>> GetFavoritosUsuario()
        {
            int userId = GetUserIdFromToken();
            // Método corregido: GetFavoritosDelUsuarioAsync
            var favoritos = await _favoritoService.GetFavoritosDelUsuarioAsync(userId);
            return Ok(favoritos);
        }

        /// <summary>
        /// Agrega un nuevo destino o lugar a la lista de favoritos del usuario.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(FavoritoDto), 201)] // Corregido a FavoritoDto
        [ProducesResponseType(400)]
        [ProducesResponseType(409)] // Conflict (si ya existe)
        public async Task<ActionResult<FavoritoDto>> PostFavorito([FromBody] FavoritoCreateDto dto) // Corregido a FavoritoCreateDto
        {
            int userId = GetUserIdFromToken();
            
            try
            {
                // Método corregido: AddFavoritoAsync
                var nuevoFavorito = await _favoritoService.AddFavoritoAsync(userId, dto);
                // Retorna 201 Created
                return CreatedAtAction(nameof(GetFavoritosUsuario), nuevoFavorito); 
            }
            catch (InvalidOperationException ex)
            {
                // Conflict 409: El favorito ya existe (buena práctica)
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
        [HttpDelete] // Usamos HTTP DELETE, lo cual es correcto.
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteFavorito([FromBody] FavoritoCreateDto dto) // Usamos el mismo DTO de entrada para identificar
        {
            int userId = GetUserIdFromToken();

            try
            {
                // Método corregido: RemoveFavoritoAsync. Usamos IdEntidad del DTO.
                // Aunque el servicio espera dos parámetros (idUsuario, idEntidad), si el DTO solo tiene idEntidad, 
                // debemos pasarlo directamente. Asumo que la eliminación será por IdEntidad.
                
                // NOTA: Para simplificar la API, usaremos los dos campos de identificación del DTO.
                var success = await _favoritoService.RemoveFavoritoAsync(userId, dto.IdEntidad);
                
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