using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace piton_taskmanagement_api.Models
{
    [BsonIgnoreExtraElements]
    public class TaskItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("title")]
        [Required]
        public string Title { get; set; } = string.Empty;

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("dueDate")]
        [Required]
        public DateTime DueDate { get; set; }

        [BsonElement("ownerId")]
        [Required]
        public string OwnerId { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "pending";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("duration")]
        [BsonRepresentation(BsonType.String)]
        [Required]
        [EnumDataType(typeof(TaskDuration))]
        public TaskDuration Duration { get; set; }
    }

    public enum TaskDuration
    {
        Daily,
        Weekly,
        Monthly
    }
}
