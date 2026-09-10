using Application.Dtos.Users;
using Application.DTOs.Users;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Users;

namespace Application.Applications.Users;

public class UserApplication : IUserApplication
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserApplication(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserResponseDto> CreateAsync(CreateUpdateUserDto input)
    {
        var user = _mapper.Map<User>(input);
        user.Email = user.Email.Trim().ToLowerInvariant();
        user.Phone = user.Phone.Trim();

        var createdUser = await _userRepository.CreateAsync(user);
        return _mapper.Map<UserResponseDto>(createdUser);
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<List<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> UpdateAsync(int id, CreateUpdateUserDto input)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        _mapper.Map(input, user);
        user.Email = user.Email.Trim().ToLowerInvariant();
        user.Phone = user.Phone.Trim();
        user.UpdatedDate = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponseDto>(updatedUser);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        await _userRepository.DeleteAsync(user);
    }
}
