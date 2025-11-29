using AutoMapper;
using Aventour.Application.DTOs;
using Aventour.Application.Services.Agencias;
using Aventour.Domain.Entities;
using Aventour.Domain.Enums;
using Aventour.Domain.Interfaces;

namespace Aventour.Application.Services
{
    public class AgenciaService : IAgenciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AgenciaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<AgenciaDto>> GetAllAsync(TipoAgenciaGuia? tipo)
        {
            var lista = await _unitOfWork.Agencias.GetAllAsync(tipo);
            return _mapper.Map<List<AgenciaDto>>(lista);
        }

        public async Task<AgenciaDto> GetByIdAsync(int id)
        {
            var entidad = await _unitOfWork.Agencias.GetByIdAsync(id);
            if (entidad == null) throw new KeyNotFoundException("Agencia o Guía no encontrada.");
            return _mapper.Map<AgenciaDto>(entidad);
        }

        public async Task<AgenciaDto> CreateAsync(AgenciaCreateDto dto)
        {
            var entidad = _mapper.Map<AgenciasGuia>(dto);
            
            // Valores por defecto
            entidad.Validado = false; 
            entidad.PuntuacionMedia = 0;

            await _unitOfWork.Agencias.AddAsync(entidad);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<AgenciaDto>(entidad);
        }

        public async Task UpdateAsync(int id, AgenciaUpdateDto dto)
        {
            var entidad = await _unitOfWork.Agencias.GetByIdAsync(id);
            if (entidad == null) throw new KeyNotFoundException("Entidad no encontrada.");

            // Actualizamos campos permitidos
            entidad.Nombre = dto.Nombre;
            entidad.WhatsappContacto = dto.WhatsappContacto;
            entidad.Email = dto.Email;
            entidad.Descripcion = dto.Descripcion;

            await _unitOfWork.CommitAsync(); // EF Core detecta cambios
        }

        public async Task DeleteAsync(int id)
        {
            var entidad = await _unitOfWork.Agencias.GetByIdAsync(id);
            if (entidad == null) throw new KeyNotFoundException("Entidad no encontrada.");

            await _unitOfWork.Agencias.DeleteAsync(entidad);
            await _unitOfWork.CommitAsync();
        }

        public async Task ValidarAgenciaAsync(int id, bool estadoValidacion)
        {
            var entidad = await _unitOfWork.Agencias.GetByIdAsync(id);
            if (entidad == null) throw new KeyNotFoundException("Entidad no encontrada.");

            entidad.Validado = estadoValidacion;
            await _unitOfWork.CommitAsync();
        }

        public async Task ActualizarPuntuacionAsync(int id)
        {
            // Llama al método del repositorio que hace el cálculo complejo
            await _unitOfWork.Agencias.RecalcularPuntuacionMediaAsync(id);
            await _unitOfWork.CommitAsync();
        }
    }
}