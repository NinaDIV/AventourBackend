using AutoMapper;
using Aventour.Application.DTOs;
using Aventour.Application.Services.Resenas;
using Aventour.Domain.Entities;
using Aventour.Domain.Enums;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models; // O Entities

namespace Aventour.Application.Services // O Services.Resenas
{
    public class ResenaService : IResenaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResenaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResenaDto> AddResenaAsync(int idUsuario, ResenaCreateDto dto)
        {
            // 1. VALIDACIÓN: ¿Existe la entidad?
            bool existe = false;

            if (dto.TipoEntidad == TipoResena.Destino)
            {
                var destino = await _unitOfWork.Destinos.GetByIdAsync(dto.IdEntidad);
                existe = (destino != null);
            }
            else if (dto.TipoEntidad == TipoResena.Agencia || dto.TipoEntidad == TipoResena.Guia)
            {
                // Asumimos que Agencias y Guias están en la misma tabla "AgenciasGuias"
                var agenciaGuia = await _unitOfWork.Agencias.GetByIdAsync(dto.IdEntidad);
                
                // Opcional: Validar que si el DTO dice "Guia", la entidad sea tipo Guia
                if (agenciaGuia != null)
                {
                    // Si tienes validación estricta de subtipo:
                    // if (agenciaGuia.Tipo.ToString() == dto.TipoEntidad.ToString()) existe = true;
                    existe = true; 
                }
            }

            if (!existe)
                throw new KeyNotFoundException($"No existe un {dto.TipoEntidad} con ID {dto.IdEntidad}.");

            // 2. VALIDACIÓN: ¿Ya reseñó esto el usuario?
            var existente = await _unitOfWork.Resenas.GetResenaEspecificaAsync(idUsuario, dto.IdEntidad, dto.TipoEntidad);
            if (existente != null)
                throw new InvalidOperationException("Ya has publicado una reseña para este elemento.");

            // 3. MAPEO Y GUARDADO
            var nuevaResena = _mapper.Map<Resena>(dto);
            nuevaResena.IdUsuario = idUsuario;
            // Aseguramos que la fecha se genere
            nuevaResena.FechaCreacion = DateTime.UtcNow; 
            // EF Core con HasConversion se encarga de TipoEntidad -> String

            await _unitOfWork.Resenas.AddResenaAsync(nuevaResena);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ResenaDto>(nuevaResena);
        }

        public async Task<List<ResenaDto>> GetResenasPorEntidadAsync(int idEntidad, TipoResena tipo)
        {
            var lista = await _unitOfWork.Resenas.GetResenasByEntidadAsync(idEntidad, tipo);
            return _mapper.Map<List<ResenaDto>>(lista);
        }

        public async Task<List<ResenaDto>> GetResenasDelUsuarioAsync(int idUsuario)
        {
            // Imprimir en consola el idUsuario para ver si es el valor correcto
            Console.WriteLine($"Consultando reseñas para el usuario con ID: {idUsuario}");

            // Obtener la lista de reseñas del usuario
            var lista = await _unitOfWork.Resenas.GetResenasByUsuarioAsync(idUsuario);

            // Verificar si la lista es null o vacía
            if (lista == null)
            {
                Console.WriteLine("No se encontraron reseñas para este usuario. Lista es null.");
                return new List<ResenaDto>();  // Retornar una lista vacía si no hay reseñas
            }

            // Verificar si la lista está vacía
            if (!lista.Any())
            {
                Console.WriteLine("No se encontraron reseñas para este usuario. Lista está vacía.");
                return new List<ResenaDto>();  // Retornar una lista vacía si no hay reseñas
            }

            // Mapear la lista de entidades a DTOs
            var result = _mapper.Map<List<ResenaDto>>(lista);

            // Imprimir en consola para asegurarse de que se mapeó correctamente
            Console.WriteLine($"Se encontraron {result.Count} reseñas para el usuario.");

            return result;
        }




        public async Task<bool> DeleteResenaAsync(int idUsuario, int idResena)
        {
            var resena = await _unitOfWork.Resenas.GetByIdAsync(idResena);

            if (resena == null) return false;

            // VALIDACIÓN: Solo el dueño puede borrar su reseña
            if (resena.IdUsuario != idUsuario)
                throw new UnauthorizedAccessException("No tienes permiso para eliminar esta reseña.");

            await _unitOfWork.Resenas.DeleteResenaAsync(resena);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}