using Aventour.Domain.Interfaces;
using Aventour.Application.DTOs;
using Aventour.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aventour.Domain.Enums;
using System;
using Aventour.Application.DTOs.Favoritos;

namespace Aventour.Application.Services.Favoritos
{
    // CASO DE USO: Centraliza la lógica de negocio para la gestión de Favoritos.
    public class GestorFavoritosService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Inyección de dependencia del Unit of Work (el contrato de persistencia)
        public GestorFavoritosService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Caso de Uso: Agregar un Favorito
        public async Task<FavoritoDTO> AgregarFavoritoAsync(int idUsuario, FavoritoInputDTO data)
        {
            // 1. Regla de Negocio: Verificar si ya existe (evitar duplicados)
            if (await _unitOfWork.Favoritos.ExistsAsync(idUsuario, data.IdEntidad, data.TipoEntidad))
            {
                throw new InvalidOperationException($"El favorito ya existe para el usuario {idUsuario}, entidad {data.IdEntidad} y tipo {data.TipoEntidad}.");
            }
            
            // 2. Creación de la Entidad de Dominio
            var nuevoFavorito = new Favorito
            {
                IdUsuario = idUsuario,
                IdEntidad = data.IdEntidad,
                TipoEntidad = data.TipoEntidad,
                FechaGuardado = DateTime.UtcNow // Se fija la fecha de guardado
            };

            // 3. Persistencia y Commit
            var favoritoPersistido = await _unitOfWork.Favoritos.AddAsync(nuevoFavorito);
            
            // Llama al Unit of Work para consolidar la transacción en la DB
            await _unitOfWork.CommitAsync(); 

            // 4. Retorno del DTO de salida
            return FavoritoDTO.FromEntity(favoritoPersistido);
        }

        // Caso de Uso: Eliminar un Favorito
        public async Task<bool> EliminarFavoritoAsync(int idUsuario, FavoritoInputDTO data)
        {
            // 1. Regla de Negocio: Buscar la entidad a eliminar
            var favorito = await _unitOfWork.Favoritos.GetByIdAsync(idUsuario, data.IdEntidad, data.TipoEntidad);

            if (favorito == null)
            {
                // Si no existe, lo tratamos como éxito (ya está eliminado)
                return true; 
            }

            // 2. Eliminación y Commit
            _unitOfWork.Favoritos.Remove(favorito);
            await _unitOfWork.CommitAsync();

            return true;
        }

        // Caso de Uso: Obtener todos los Favoritos de un Usuario
        public async Task<IEnumerable<FavoritoDTO>> ObtenerFavoritosPorUsuarioAsync(int idUsuario)
        {
            var favoritos = await _unitOfWork.Favoritos.GetByUserIdAsync(idUsuario);

            // Mapeo de la colección de Entidades a DTOs
            return favoritos.Select(FavoritoDTO.FromEntity);
        }
    }
}