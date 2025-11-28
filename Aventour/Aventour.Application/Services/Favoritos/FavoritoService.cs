using AutoMapper;
using Aventour.Application.DTOs;
using Aventour.Application.UseCases.Favoritos;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;

namespace Aventour.Application.Services.Favoritos;

public class FavoritoService : IFavoritoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FavoritoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<FavoritoDto>> GetFavoritosDelUsuarioAsync(int idUsuario)
    {
        var favoritos = await _unitOfWork.Favoritos.GetFavoritosByUsuarioAsync(idUsuario);
        return _mapper.Map<List<FavoritoDto>>(favoritos);
    }

    public async Task<FavoritoDto> AddFavoritoAsync(int idUsuario, FavoritoCreateDto favoritoDto)
    {
        // 1. Verificar si ya existe (para evitar duplicados)
        var existingFav = await _unitOfWork.Favoritos.GetFavoritoAsync(idUsuario, favoritoDto.IdEntidad);
        if (existingFav != null)
        {
            // Podrías lanzar una excepción o devolver el existente
            return _mapper.Map<FavoritoDto>(existingFav);
        }
            
        // 2. Mapear DTO a Entidad
        var nuevoFavorito = _mapper.Map<Favorito>(favoritoDto);
        nuevoFavorito.IdUsuario = idUsuario;
            
        // 3. Añadir y Guardar
        await _unitOfWork.Favoritos.AddFavoritoAsync(nuevoFavorito);
        await _unitOfWork.CommitAsync();
            
        return _mapper.Map<FavoritoDto>(nuevoFavorito);
    }

    public async Task<bool> RemoveFavoritoAsync(int idUsuario, int idEntidad)
    {
        var favorito = await _unitOfWork.Favoritos.GetFavoritoAsync(idUsuario, idEntidad);
            
        if (favorito == null)
        {
            return false; // El favorito no existe
        }

        _unitOfWork.Favoritos.DeleteFavoritoAsync(favorito);
        await _unitOfWork.CommitAsync();
            
        return true;
    }
}