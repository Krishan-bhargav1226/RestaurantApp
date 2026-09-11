using Application.Dtos.PasswordResetOTPs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.PasswordResetOTPs;

namespace Application.Applications.PasswordResetOTPs;

public class PasswordResetOTPApplication : IPasswordResetOTPApplication
{
    private readonly IPasswordResetOTPRepository _passwordResetOTPRepository;
    private readonly IMapper _mapper;

    public PasswordResetOTPApplication(IPasswordResetOTPRepository passwordResetOTPRepository, IMapper mapper)
    {
        _passwordResetOTPRepository = passwordResetOTPRepository;
        _mapper = mapper;
    }

    public async Task<PasswordResetOTPResponseDto> CreateAsync(CreateUpdatePasswordResetOTPDto input)
    {
        var passwordResetOTP = _mapper.Map<PasswordResetOTP>(input);

        var createdPasswordResetOTP = await _passwordResetOTPRepository.CreateAsync(passwordResetOTP);

        return _mapper.Map<PasswordResetOTPResponseDto>(createdPasswordResetOTP);
    }

    public async Task<List<PasswordResetOTPResponseDto>> GetAllAsync()
    {
        var passwordResetOTPs = await _passwordResetOTPRepository.GetAllAsync();

        return _mapper.Map<List<PasswordResetOTPResponseDto>>(passwordResetOTPs);
    }

    public async Task<PasswordResetOTPResponseDto> GetByIdAsync(int id)
    {
        var passwordResetOTP = await _passwordResetOTPRepository.GetByIdAsync(id);

        if (passwordResetOTP == null)
            throw new KeyNotFoundException("Password reset OTP not found.");

        return _mapper.Map<PasswordResetOTPResponseDto>(passwordResetOTP);
    }

    public async Task<PasswordResetOTPResponseDto> UpdateAsync(int id, CreateUpdatePasswordResetOTPDto input)
    {
        var passwordResetOTP = await _passwordResetOTPRepository.GetByIdAsync(id);

        if (passwordResetOTP == null)
            throw new KeyNotFoundException("Password reset OTP not found.");

        _mapper.Map(input, passwordResetOTP);
        passwordResetOTP.UpdatedDate = DateTime.UtcNow;

        var updatedPasswordResetOTP = await _passwordResetOTPRepository.UpdateAsync(passwordResetOTP);

        return _mapper.Map<PasswordResetOTPResponseDto>(updatedPasswordResetOTP);
    }

    public async Task DeleteAsync(int id)
    {
        var passwordResetOTP = await _passwordResetOTPRepository.GetByIdAsync(id);

        if (passwordResetOTP == null)
            throw new KeyNotFoundException("Password reset OTP not found.");

        await _passwordResetOTPRepository.DeleteAsync(passwordResetOTP);
    }
}