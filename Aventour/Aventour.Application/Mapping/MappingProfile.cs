using AutoMapper;
using Aventour.Application.DTOs;
using Aventour.Domain.Entities;
using Aventour.Domain.Models; // O Entities, según tu estructura

namespace Aventour.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeos de Reseñas
            CreateMap<ResenaCreateDto, Resena>();
            CreateMap<Resena, ResenaDto>()
                .ForMember(
                    d => d.NombreUsuario, 
                    o => o.MapFrom(s => s.IdUsuarioNavigation.Nombres)
                );
            
            CreateMap<Favorito, FavoritoDto>()
                .ForMember(dest => dest.TipoEntidad, opt => opt.MapFrom(src => src.TipoEntidad.ToString()));

            CreateMap<FavoritoCreateDto, Favorito>();
            
            CreateMap<AgenciaCreateDto, AgenciasGuia>();
            CreateMap<AgenciasGuia, AgenciaDto>();

            // Aquí puedes agregar más mapeos (Favoritos, Destinos, etc.)
        }
        
        
    }
}