using piton_taskmanagement_api.Models;

namespace piton_taskmanagement_api.DTOs.Task
{
    public class TaskDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "pending";
        public TaskDuration Duration { get; set; }
    }
}
