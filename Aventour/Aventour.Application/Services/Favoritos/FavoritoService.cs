using Aventour.Application.DTOs.Favoritos;
using Aventour.Application.Interfaces;
using Aventour.Domain.Entities;
using Aventour.Domain.Interfaces;

namespace Aventour.Application.Services
{
    public class FavoritoService : IFavoritoService
    {
        private readonly IUnitOfWork _unitOfWork; 
        // Asumiendo que usas UnitOfWork, si no, inyecta IFavoritoRepository directamente

        public FavoritoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FavoritoResponseDto>> ObtenerFavoritosUsuario(int idUsuario)
        {
            // El repositorio nos devolverá una lista con datos combinados (Favorito + Destino)
            var resultados = await _unitOfWork.FavoritoRepository.ListarFavoritosPorUsuario(idUsuario);

            var listaDtos = new List<FavoritoResponseDto>();

            foreach (var item in resultados)
            {
                // Mapeo manual o usando AutoMapper
                // Nota: 'item' es dynamic o un objeto anónimo devuelto por el repo
                listaDtos.Add(new FavoritoResponseDto
                {
                    IdDestino = item.Destino.IdDestino,
                    NombreDestino = item.Destino.Nombre,
                    DescripcionBreve = item.Destino.DescripcionBreve,
                    UrlFotoPrincipal = item.Destino.UrlFotoPrincipal,
                    FechaGuardado = item.Favorito.FechaGuardado
                });
            }

            return listaDtos;
        }

        public async Task<bool> AgregarFavorito(int idUsuario, CrearFavoritoDto dto)
        {
            var existe = await _unitOfWork.FavoritoRepository.ExisteFavorito(idUsuario, dto.IdDestino);
            if (existe) return false; // Ya existe, no hacemos nada

            var nuevoFavorito = new Favorito
            {
                IdUsuario = idUsuario,
                IdEntidad = dto.IdDestino, // IdEntidad actúa como IdDestino
                FechaGuardado = DateTime.UtcNow
            };

            await _unitOfWork.FavoritoRepository.AgregarFavorito(nuevoFavorito);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarFavorito(int idUsuario, int idDestino)
        {
            var existe = await _unitOfWork.FavoritoRepository.ExisteFavorito(idUsuario, idDestino);
            if (!existe) return false;

            await _unitOfWork.FavoritoRepository.EliminarFavorito(idUsuario, idDestino);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}