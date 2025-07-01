using piton_taskmanagement_api.DTOs.Task;
using piton_taskmanagement_api.Models;
using piton_taskmanagement_api.Repositories.Interfaces;
using piton_taskmanagement_api.Services.Interfaces;

namespace piton_taskmanagement_api.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<TaskDto>> GetAllAsync(string userId)
        {
            var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
            return tasks.Select(ToDto).ToList();
        }

        public async Task<List<TaskDto>> GetDailyAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;
            return await GetByRange(userId, today, today.AddDays(1).AddTicks(-1));
        }

        public async Task<List<TaskDto>> GetWeeklyAsync(string userId)
        {
            var today = DateTime.UtcNow;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var start = today.Date.AddDays(-1 * diff);
            var end = start.AddDays(7).AddTicks(-1);
            return await GetByRange(userId, start, end);
        }

        public async Task<List<TaskDto>> GetMonthlyAsync(string userId)
        {
            var now = DateTime.UtcNow;
            var start = new DateTime(now.Year, now.Month, 1);
            var end = start.AddMonths(1).AddTicks(-1);
            return await GetByRange(userId, start, end);
        }

        private async Task<List<TaskDto>> GetByRange(string userId, DateTime start, DateTime end)
        {
            var tasks = await _taskRepository.GetByDateRangeAsync(userId, start, end);
            return tasks.Select(ToDto).ToList();
        }

        public async Task<TaskDto?> GetByIdAsync(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task == null ? null : ToDto(task);
        }

        public async Task<TaskDto> CreateAsync(string userId, CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                OwnerId = userId,
                Status = "pending"
            };

            await _taskRepository.CreateAsync(task);
            return ToDto(task);
        }

        public async Task UpdateAsync(string id, CreateTaskDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;

            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteAsync(string id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        private static TaskDto ToDto(TaskItem task) => new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status
        };
    }
}
