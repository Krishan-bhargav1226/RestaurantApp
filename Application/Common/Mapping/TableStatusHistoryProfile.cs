using Application.Dtos.TableStatusHistories;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class TableStatusHistoryProfile : Profile
{
    public TableStatusHistoryProfile()
    {
        CreateMap<CreateUpdateTableStatusHistoryDto, TableStatusHistory>();
        CreateMap<TableStatusHistory, TableStatusHistoryResponseDto>();
    }
}
