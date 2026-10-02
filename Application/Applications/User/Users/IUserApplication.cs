using Application.Dtos.Users;

namespace Application.Applications.Users;

public interface IUserApplication
{
    Task<UserResponseDto> CreateAsync(CreateUpdateUserDto input);
    Task<List<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> GetByIdAsync(int id);
    Task<UserResponseDto> UpdateAsync(int id, CreateUpdateUserDto input);
    Task DeleteAsync(int id);
}
