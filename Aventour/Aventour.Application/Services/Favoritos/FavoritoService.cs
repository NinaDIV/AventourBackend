using AutoMapper;
using Aventour.Application.DTOs;
using Aventour.Domain.Enums;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;

namespace Aventour.Application.Services.Favoritos // O UseCases
{
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
            var lista = await _unitOfWork.Favoritos.GetFavoritosByUsuarioAsync(idUsuario);
            return _mapper.Map<List<FavoritoDto>>(lista);
        }

        public async Task<FavoritoDto> AddFavoritoAsync(int idUsuario, FavoritoCreateDto dto)
        {
            // 1. VALIDACIÓN: ¿Existe la entidad que queremos guardar?
            bool existe = false;
            if (dto.TipoEntidad == TipoFavorito.Destino)
            {
                var destino = await _unitOfWork.Destinos.GetByIdAsync(dto.IdEntidad);
                existe = (destino != null);
            }
            else if (dto.TipoEntidad == TipoFavorito.Lugar)
            {
                // TODO: Cuando implementes IHotelesRepository, descomenta esto:
                // var lugar = await _unitOfWork.Hoteles.GetByIdAsync(dto.IdEntidad);
                // existe = (lugar != null);
                existe = true; // Temporal para que no falle si pruebas 'Lugar' ahora
            }

            if (!existe)
                throw new KeyNotFoundException($"No existe un {dto.TipoEntidad} con ID {dto.IdEntidad}.");

            // 2. VALIDACIÓN: ¿Ya es favorito?
            var existente = await _unitOfWork.Favoritos.GetFavoritoAsync(idUsuario, dto.IdEntidad, dto.TipoEntidad);
            if (existente != null)
                throw new InvalidOperationException("Este elemento ya está en tus favoritos.");

            // 3. GUARDAR
            var nuevoFavorito = _mapper.Map<Favorito>(dto);
            nuevoFavorito.IdUsuario = idUsuario; // Asignamos el ID que vino del Token
            
            await _unitOfWork.Favoritos.AddFavoritoAsync(nuevoFavorito);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<FavoritoDto>(nuevoFavorito);
        }

        public async Task<bool> RemoveFavoritoAsync(int idUsuario, int idEntidad, TipoFavorito tipo)
        {
            var favorito = await _unitOfWork.Favoritos.GetFavoritoAsync(idUsuario, idEntidad, tipo);
            if (favorito == null) return false;

            await _unitOfWork.Favoritos.DeleteFavoritoAsync(favorito);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}