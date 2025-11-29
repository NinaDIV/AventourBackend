using Aventour.Application.DTOs;
using Aventour.Application.Services.Agencias;
using Aventour.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgenciasController : ControllerBase
    {
        private readonly IAgenciaService _agenciaService;

        public AgenciasController(IAgenciaService agenciaService)
        {
            _agenciaService = agenciaService;
        }

        // GET: api/v1/Agencias (Opcional: ?tipo=Guia)
        [HttpGet]
        public async Task<ActionResult<List<AgenciaDto>>> GetAll([FromQuery] TipoAgenciaGuia? tipo)
        {
            var lista = await _agenciaService.GetAllAsync(tipo);
            return Ok(lista);
        }

        // GET: api/v1/Agencias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgenciaDto>> GetById(int id)
        {
            try
            {
                var dto = await _agenciaService.GetByIdAsync(id);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/v1/Agencias
        [HttpPost]
        [Authorize] // Requiere login
        public async Task<ActionResult<AgenciaDto>> Create([FromBody] AgenciaCreateDto dto)
        {
            var creado = await _agenciaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.IdAgencia }, creado);
        }

        // PUT: api/v1/Agencias/5
        [HttpPut("{id}")]
        [Authorize] 
        public async Task<IActionResult> Update(int id, [FromBody] AgenciaUpdateDto dto)
        {
            if (id != dto.IdAgencia) return BadRequest("ID no coincide.");

            try
            {
                await _agenciaService.UpdateAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PATCH: api/v1/Agencias/5/validar
        // Endpoint exclusivo para Administradores
        [HttpPatch("{id}/validar")]
        [Authorize(Roles = "Admin")] // Asumiendo que usas Roles
        public async Task<IActionResult> Validar(int id, [FromBody] bool validar)
        {
            try
            {
                await _agenciaService.ValidarAgenciaAsync(id, validar);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/v1/Agencias/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Solo admins borran agencias
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _agenciaService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}