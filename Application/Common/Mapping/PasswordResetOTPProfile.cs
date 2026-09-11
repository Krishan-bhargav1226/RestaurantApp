using Application.Dtos.PasswordResetOTPs;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapping;

public class PasswordResetOTPProfile : Profile
{
    public PasswordResetOTPProfile()
    {
        CreateMap<CreateUpdatePasswordResetOTPDto, PasswordResetOTP>();
        CreateMap<PasswordResetOTP, PasswordResetOTPResponseDto>();
    }
}