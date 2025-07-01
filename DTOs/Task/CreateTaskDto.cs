using System.ComponentModel.DataAnnotations;
using piton_taskmanagement_api.Models;
using System.Text.Json.Serialization;

namespace piton_taskmanagement_api.DTOs.Task
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskDuration Duration { get; set; }
    }
}