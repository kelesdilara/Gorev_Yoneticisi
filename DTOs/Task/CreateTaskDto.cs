using System.ComponentModel.DataAnnotations;

namespace piton_taskmanagement_api.DTOs.Task
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
