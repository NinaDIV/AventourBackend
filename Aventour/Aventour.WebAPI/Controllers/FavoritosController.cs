using Aventour.Application.DTOs.Favoritos;
using Aventour.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aventour.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requiere token JWT
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritoService _favoritoService;

        public FavoritosController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        // Helper para obtener ID del token
        private int ObtenerIdUsuarioLogueado()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                // Ajusta "uid" o "id" según como hayas configurado tu JWT
                var idClaim = userClaims.FirstOrDefault(x => x.Type == "uid")?.Value; 
                if (int.TryParse(idClaim, out int id)) return id;
            }
            return 0;
        }

        [HttpGet]
        public async Task<IActionResult> ListarFavoritos()
        {
            int idUsuario = ObtenerIdUsuarioLogueado();
            if (idUsuario == 0) return Unauthorized("Usuario no identificado");

            var resultado = await _favoritoService.ObtenerFavoritosUsuario(idUsuario);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarFavorito([FromBody] CrearFavoritoDto dto)
        {
            int idUsuario = ObtenerIdUsuarioLogueado();
            if (idUsuario == 0) return Unauthorized("Usuario no identificado");

            var exito = await _favoritoService.AgregarFavorito(idUsuario, dto);
            
            if (!exito) return Conflict(new { mensaje = "El destino ya está en favoritos" });

            return Ok(new { mensaje = "Favorito agregado correctamente" });
        }

        [HttpDelete("{idDestino}")]
        public async Task<IActionResult> EliminarFavorito(int idDestino)
        {
            int idUsuario = ObtenerIdUsuarioLogueado();
            if (idUsuario == 0) return Unauthorized("Usuario no identificado");

            var exito = await _favoritoService.EliminarFavorito(idUsuario, idDestino);

            if (!exito) return NotFound(new { mensaje = "Favorito no encontrado" });

            return Ok(new { mensaje = "Favorito eliminado correctamente" });
        }
    }
}