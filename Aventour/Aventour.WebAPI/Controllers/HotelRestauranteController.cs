using Aventour.Application.DTOs.Lugares;
using Aventour.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aventour.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelRestauranteController : ControllerBase
    {
        private readonly HotelRestauranteService _service;

        public HotelRestauranteController(HotelRestauranteService service)
        {
            _service = service;
        }

        // GET: api/HotelRestaurante
        // Permite filtrar: api/HotelRestaurante?tipo=Lugar
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? tipo)
        {
            // Validación del tipo, que puede ser 'Lugar' o 'Destino'
            if (!string.IsNullOrEmpty(tipo) && tipo != "Lugar" && tipo != "Destino")
            {
                return BadRequest(new { message = "Tipo inválido. Debe ser 'Lugar' o 'Destino'." });
            }

            // Llamamos al servicio para obtener los lugares filtrados
            var lista = await _service.ListarLugaresAsync(tipo);
            return Ok(lista);
        }

        // GET: api/HotelRestaurante/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            try
            {
                var lugar = await _service.ObtenerPorIdAsync(id);
                return Ok(lugar);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/HotelRestaurante (Solo Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear([FromBody] CrearLugarDto dto)
        {
            try
            {
                var id = await _service.CrearLugarAsync(dto);
                return CreatedAtAction(nameof(Obtener), new { id = id }, new { id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/HotelRestaurante/{id} (Solo Admin)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateLugarDto dto)
        {
            try
            {
                await _service.ActualizarLugarAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/HotelRestaurante/{id} (Solo Admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _service.EliminarLugarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
