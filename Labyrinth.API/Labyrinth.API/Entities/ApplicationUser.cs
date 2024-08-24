using Labyrinth.API.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Labyrinth.API.Entities
{
    public class ApplicationUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // This will map to the MongoDB _id field

        [Key]
        public string UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string DisplayName { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; } // Enum property

        [MaxLength(500)]
        public string Description { get; set; } // User bio/description

        // Optional: settings, preferences, etc.
        public UserSettings Settings { get; set; } // Another class to store user-specific settings
    }
}

