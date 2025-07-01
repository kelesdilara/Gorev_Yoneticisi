using piton_taskmanagement_api.DTOs.User;

namespace piton_taskmanagement_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterAsync(CreateUserDto createUserDto);
        Task<string?> LoginAsync(LoginUserDto loginUserDto);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}
