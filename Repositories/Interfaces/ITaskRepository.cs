using piton_taskmanagement_api.Models;

namespace piton_taskmanagement_api.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllByUserIdAsync(string userId);
        Task<List<TaskItem>> GetByDateRangeAsync(string userId, DateTime start, DateTime end);
        Task<TaskItem?> GetByIdAsync(string id);
        Task CreateAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(string id);
    }
}
