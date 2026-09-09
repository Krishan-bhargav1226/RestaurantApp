using Application.Dtos.Products;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateUpdateProductDto, Product>();

            CreateMap<Product, ProductResponseDto>();
        }
    }
}
