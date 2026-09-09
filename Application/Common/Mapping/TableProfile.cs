using Application.Dtos.Tables;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class TableProfile : Profile
{
    public TableProfile()
    {
        CreateMap<CreateUpdateTableDto, Table>();
        CreateMap<Table, TableResponseDto>();
    }
}
