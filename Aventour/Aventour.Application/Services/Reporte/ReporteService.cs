using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces; // Asumiendo que aquí están tus repositorios
using Aventour.Application.Services.Destinos; // Para IDestinoService
using ClosedXML.Excel;
using System.IO;

namespace Aventour.Application.Services
{
    public class ReporteService
    {
        // Inyectamos los repositorios/servicios necesarios para obtener la data
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IDestinoService _destinoService; 
        private readonly IPackRutaRepository _packRepository;

        public ReporteService(
            IUsuarioRepository usuarioRepository, 
            IDestinoService destinoService,
            IPackRutaRepository packRepository)
        {
            _usuarioRepository = usuarioRepository;
            _destinoService = destinoService;
            _packRepository = packRepository;
        }

        // --- MÉTODO GENÉRICO PARA CREAR EL ARCHIVO EXCEL ---
        private byte[] GenerarExcel(Action<IXLWorkbook> action)
        {
            using (var workbook = new XLWorkbook())
            {
                action(workbook);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        // 1. REPORTE DE USUARIOS
        public async Task<byte[]> GenerarReporteUsuariosAsync()
        {
            // Asumimos que tienes un método GetAllAsync o Listar
            var usuarios = await _usuarioRepository.GetAllAsync(); 
            
            return GenerarExcel(workbook =>
            {
                var worksheet = workbook.Worksheets.Add("Usuarios");

                // Cabeceras
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nombres";
                worksheet.Cell(1, 3).Value = "Apellidos";
                worksheet.Cell(1, 4).Value = "Email";
                worksheet.Cell(1, 5).Value = "Rol";
                worksheet.Cell(1, 6).Value = "Fecha Registro";
                
                // Estilo Cabecera
                var cabecera = worksheet.Range("A1:F1");
                cabecera.Style.Font.Bold = true;
                cabecera.Style.Fill.BackgroundColor = XLColor.LightBlue;

                // Datos
                int row = 2;
                foreach (var u in usuarios)
                {
                    worksheet.Cell(row, 1).Value = u.IdUsuario;
                    worksheet.Cell(row, 2).Value = u.Nombres;
                    worksheet.Cell(row, 3).Value = u.Apellidos;
                    worksheet.Cell(row, 4).Value = u.Email;
                    worksheet.Cell(row, 5).Value = (u.EsAdministrador == true) ? "Admin" : "Turista";
                    worksheet.Cell(row, 6).Value = u.FechaRegistro?.ToString("dd/MM/yyyy");
                    row++;
                }

                worksheet.Columns().AdjustToContents();
            });
        }

        // 2. REPORTE DE DESTINOS
        public async Task<byte[]> GenerarReporteDestinosAsync()
        {
            var destinos = await _destinoService.ListarTodos(); // Devuelve DTOs

            return GenerarExcel(workbook =>
            {
                var worksheet = workbook.Worksheets.Add("Destinos Turísticos");

                // Cabeceras
                worksheet.Cell(1, 1).Value = "Nombre";
                worksheet.Cell(1, 2).Value = "Tipo";
                worksheet.Cell(1, 3).Value = "Costo Entrada";
                worksheet.Cell(1, 4).Value = "Puntuación";
                worksheet.Cell(1, 5).Value = "Descripción Breve";

                var cabecera = worksheet.Range("A1:E1");
                cabecera.Style.Font.Bold = true;
                cabecera.Style.Fill.BackgroundColor = XLColor.LightGreen;

                int row = 2;
                foreach (var d in destinos)
                {
                    worksheet.Cell(row, 1).Value = d.Nombre;
                    worksheet.Cell(row, 2).Value = d.Tipo;
                    worksheet.Cell(row, 3).Value = d.CostoEntrada;
                    worksheet.Cell(row, 4).Value = d.PuntuacionMedia;
                    worksheet.Cell(row, 5).Value = d.DescripcionBreve;
                    row++;
                }

                worksheet.Columns().AdjustToContents();
            });
        }

        // 3. REPORTE DE PACKS DE AGENCIA
        public async Task<byte[]> GenerarReportePacksAsync()
        {
            var packs = await _packRepository.GetAllAsync(); // Devuelve Entidades

            return GenerarExcel(workbook =>
            {
                var worksheet = workbook.Worksheets.Add("Packs Agencias");

                worksheet.Cell(1, 1).Value = "Agencia";
                worksheet.Cell(1, 2).Value = "Nombre Pack";
                worksheet.Cell(1, 3).Value = "Precio";
                worksheet.Cell(1, 4).Value = "Duración (Días)";
                worksheet.Cell(1, 5).Value = "Cant. Destinos";

                var cabecera = worksheet.Range("A1:E1");
                cabecera.Style.Font.Bold = true;
                cabecera.Style.Fill.BackgroundColor = XLColor.Orange;

                int row = 2;
                foreach (var p in packs)
                {
                    // Nota: Asegúrate de incluir la navegación en tu repositorio (.Include(x => x.IdAgenciaNavigation))
                    worksheet.Cell(row, 1).Value = p.IdAgenciaNavigation?.Nombre ?? "N/A";
                    worksheet.Cell(row, 2).Value = p.NombrePack;
                    worksheet.Cell(row, 3).Value = p.PrecioBase;
                    worksheet.Cell(row, 4).Value = p.DuracionDias;
                    worksheet.Cell(row, 5).Value = p.DetallePackDestinos.Count;
                    row++;
                }

                worksheet.Columns().AdjustToContents();
            });
        }
    }
}