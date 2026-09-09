using Application.Dtos.Ingredients;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, IngredientResponseDto>();
            CreateMap<CreateUpdateIngredientDto, Ingredient>();
        }
    }
}
