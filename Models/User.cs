using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace piton_taskmanagement_api.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("username")]
        [Required]
        public string Username { get; set; } = null!;

        [BsonElement("email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [BsonElement("passwordHash")]  // Bu satırı ekleyin
        [Required]                     // Bu satırı ekleyin
        public string PasswordHash { get; set; } = null!;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
