using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile 
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>().ReverseMap();
            CreateMap<CategoryEntity, CategoryDto>().ReverseMap();  
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Owner, OwnerDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Reviewer, ReviewerDto>().ReverseMap();
            //CreateMap<Pokemon, PokemonDto>().ForMember(dest => dest.BirthDate,
                //opt => opt.MapFrom(src => src.BirthDate)); // Кастомный конфиг автомапера
        }
    }
}
