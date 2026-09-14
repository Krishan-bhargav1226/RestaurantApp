using Application.Dtos.TableSessions;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class TableSessionProfile : Profile
{
    public TableSessionProfile()
    {
        CreateMap<CreateUpdateTableSessionDto, TableSession>();

        CreateMap<TableSession, TableSessionResponseDto>();
    }
}