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

        private async Task<List<TaskDto>> GetByDurationAsync(string userId, TaskDuration duration)
        {
            var tasks = await _taskRepository.GetAllByUserIdAsync(userId);
            return tasks
                .Where(t => t.Duration == duration)
                .Select(ToDto)
                .ToList();
        }

        public Task<List<TaskDto>> GetDailyAsync(string userId) =>
            GetByDurationAsync(userId, TaskDuration.Daily);

        public Task<List<TaskDto>> GetWeeklyAsync(string userId) =>
            GetByDurationAsync(userId, TaskDuration.Weekly);

        public Task<List<TaskDto>> GetMonthlyAsync(string userId) =>
            GetByDurationAsync(userId, TaskDuration.Monthly);

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
                Status = "pending",
                Duration = dto.Duration
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
            task.Duration = dto.Duration;

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
            Status = task.Status,
            Duration = task.Duration
        };
    }
}
