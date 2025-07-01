using piton_taskmanagement_api.DTOs.Task;

namespace piton_taskmanagement_api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllAsync(string userId);
        Task<List<TaskDto>> GetDailyAsync(string userId);
        Task<List<TaskDto>> GetWeeklyAsync(string userId);
        Task<List<TaskDto>> GetMonthlyAsync(string userId);
        Task<TaskDto?> GetByIdAsync(string id);
        Task<TaskDto> CreateAsync(string userId, CreateTaskDto dto);
        Task UpdateAsync(string id, CreateTaskDto dto);
        Task DeleteAsync(string id);
    }
}
