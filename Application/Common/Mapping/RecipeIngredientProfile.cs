using Application.Dtos.RecipeIngredients;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    public class RecipeIngredientProfile : Profile
    {
        public RecipeIngredientProfile()
        {
            CreateMap<RecipeIngredient, RecipeIngredientResponseDto>();
            CreateMap<CreateUpdateRecipeIngredientDto, RecipeIngredient>();
        }
    }
}
