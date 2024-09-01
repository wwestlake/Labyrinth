using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Storyline
{
    public class StoryCharacter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Role { get; set; }
        public string Affiliation { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
    }
}
