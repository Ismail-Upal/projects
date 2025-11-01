using AutoMapper;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api.Mapping;

public class GenreMappingProfile : Profile
{
    public GenreMappingProfile()
    {
        CreateMap<Genre, GenreDto>();
        CreateMap<GenreDto, Genre>();
    }
}