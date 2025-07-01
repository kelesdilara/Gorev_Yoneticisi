using piton_taskmanagement_api.Models;

namespace piton_taskmanagement_api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
