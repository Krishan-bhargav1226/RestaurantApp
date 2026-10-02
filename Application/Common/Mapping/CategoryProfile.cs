using Application.Dtos.Categories;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateUpdateCategoryDto, Category>();

            CreateMap<Category, CategoryResponseDto>();
        }
    }
}