using Application.Dtos.BranchProducts;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping
{
    public class BranchProductProfile : Profile
    {
        public BranchProductProfile()
        {
            CreateMap<CreateUpdateBranchProductDto, BranchProduct>();

            CreateMap<BranchProduct, BranchProductResponseDto>();
        }
    }
}
