// Aventour.Application/Mapping/FavoritoMappingProfile.cs (o similar)

using AutoMapper;
using Aventour.Application.DTOs;
using Aventour.Domain.Models;
using System;

namespace Aventour.Application.Mapping
{
    public class FavoritoMappingProfile : Profile
    {
        public FavoritoMappingProfile()
        {
            // Mapeo de la Entidad Favorito (Dominio) a FavoritoDto (Aplicación/Salida)
            CreateMap<Favorito, FavoritoDto>();

            // Mapeo de FavoritoCreateDto (Aplicación/Entrada) a la Entidad Favorito (Dominio)
            // Esto se usa al AGREGAR un nuevo favorito.
            CreateMap<FavoritoCreateDto, Favorito>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore()) // Lo ignoramos ya que se asigna en el servicio/caso de uso
                .ForMember(dest => dest.FechaGuardado, opt => opt.Ignore()); // Lo ignoramos ya que la BD/EF Core le pone el valor por defecto 'now()'
        }
    }
}