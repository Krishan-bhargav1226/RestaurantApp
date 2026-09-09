using Application.Dtos.Categories;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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