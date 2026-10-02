using Application.Dtos.Users;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUpdateUserDto, User>();
        CreateMap<User, UserResponseDto>();
    }
}
