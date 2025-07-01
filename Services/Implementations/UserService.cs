using piton_taskmanagement_api.DTOs.User;
using piton_taskmanagement_api.Models;
using piton_taskmanagement_api.Repositories.Interfaces;
using piton_taskmanagement_api.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace piton_taskmanagement_api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterAsync(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already registered.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = ComputeSha256Hash(dto.Password);

            await _userRepository.CreateAsync(user);

            return _jwtService.GenerateToken(user); // register sonrası da token döner
        }

        public async Task<string?> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null) return null;

            var hashedPassword = ComputeSha256Hash(dto.Password);
            if (user.PasswordHash != hashedPassword)
                return null;

            return _jwtService.GenerateToken(user);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            }).ToList();
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }

        public async Task DeleteAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
        }

        // SHA256 Hash Fonksiyonu
        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
