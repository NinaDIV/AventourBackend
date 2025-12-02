using Aventour.Application.DTOs.Destinos;
using Aventour.Application.Repositories;
using Aventour.Application.Services.Destinos;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinosController : ControllerBase
    {
        private readonly IDestinoService _destinoService;
        public DestinosController(IDestinoService destinoService)
        {
            _destinoService = destinoService;
        }
        
        // PATCH: api/Destinos/puntuacion
        [HttpPatch("{id}/puntuacion")]
        public async Task<IActionResult> ActualizarPuntuacionMedia(int id)
        {
            try
            {
                await _destinoService.ActualizarPuntuacionMedia(id);
                return NoContent(); // 204 OK
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró el destino con ID {id}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar la puntuación: {ex.Message}");
            }
        }


        // GET: api/Destinos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DestinoResponseDto>>> ListarTodos()
        {
            var destinos = await _destinoService.ListarTodos();
            return Ok(destinos);
        }

        // GET: api/Destinos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DestinoResponseDto>> ObtenerPorId(int id)
        {
            try
            {
                var destino = await _destinoService.ObtenerPorId(id);
                return Ok(destino);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró el destino con ID {id}");
            }
        }

        // POST: api/Destinos
        [HttpPost]
        public async Task<ActionResult<int>> Crear([FromBody] CrearDestinoDto crearDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var id = await _destinoService.Crear(crearDto);
                // Retorna un 201 Created y la URL para consultar el recurso creado
                return CreatedAtAction(nameof(ObtenerPorId), new { id = id }, new { id = id });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el destino: {ex.Message}");
            }
        }

        // PUT: api/Destinos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateDestinoDto updateDto)
        {
            if (id != updateDto.IdDestino)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo.");
            }

            try
            {
                await _destinoService.Actualizar(updateDto);
                return NoContent(); // 204 No Content (éxito sin cuerpo de respuesta)
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró el destino con ID {id} para actualizar.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar: {ex.Message}");
            }
        }

        // DELETE: api/Destinos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _destinoService.Eliminar(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró el destino con ID {id} para eliminar.");
            }
        }
    }
}