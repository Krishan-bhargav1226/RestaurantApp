using Application.DTOs.Branches;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<CreateUpdateBranchDto, Branch>();
        CreateMap<Branch, BranchResponseDto>();
    }
}