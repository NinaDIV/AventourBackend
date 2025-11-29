using Aventour.Application.DTOs;
using Aventour.Domain.Enums;

namespace Aventour.Application.Services.Resenas
{
    public interface IResenaService
    {
        Task<ResenaDto> AddResenaAsync(int idUsuario, ResenaCreateDto dto);
        Task<List<ResenaDto>> GetResenasPorEntidadAsync(int idEntidad, TipoResena tipo);
        Task<List<ResenaDto>> GetResenasDelUsuarioAsync(int idUsuario);
        Task<bool> DeleteResenaAsync(int idUsuario, int idResena);
    }
}