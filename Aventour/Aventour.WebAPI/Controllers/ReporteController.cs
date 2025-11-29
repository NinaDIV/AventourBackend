using Aventour.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")] // Opcional: Proteger para que solo admins descarguen reportes
    public class ReporteController : ControllerBase
    {
        private readonly ReporteService _reporteService;

        public ReporteController(ReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> DescargarReporteUsuarios()
        {
            try
            {
                var archivo = await _reporteService.GenerarReporteUsuariosAsync();
                string nombreArchivo = $"Usuarios_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                string tipoContenido = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(archivo, tipoContenido, nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al generar el reporte: " + ex.Message });
            }
        }

        [HttpGet("destinos")]
        public async Task<IActionResult> DescargarReporteDestinos()
        {
            try
            {
                var archivo = await _reporteService.GenerarReporteDestinosAsync();
                string nombreArchivo = $"Destinos_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                string tipoContenido = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(archivo, tipoContenido, nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("packs")]
        public async Task<IActionResult> DescargarReportePacks()
        {
            try
            {
                var archivo = await _reporteService.GenerarReportePacksAsync();
                string nombreArchivo = $"PacksAgencia_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                string tipoContenido = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(archivo, tipoContenido, nombreArchivo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}