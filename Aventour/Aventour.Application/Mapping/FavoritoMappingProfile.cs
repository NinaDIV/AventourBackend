using AutoMapper;
using Aventour.Application.DTOs;
using Aventour.Domain.Models;

namespace Aventour.Application.Mapping
{
    public class FavoritoMappingProfile : Profile
    {
        public FavoritoMappingProfile()
        {
            CreateMap<Favorito, FavoritoDto>()
                .ForMember(dest => dest.TipoEntidad, opt => opt.MapFrom(src => src.TipoEntidad.ToString()));

            CreateMap<FavoritoCreateDto, Favorito>();
        }
    }
}