using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Labyrinth.API.Entities.Storyline
{
    public class UserProgress
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PlayerId { get; set; }
        public List<string> ChaptersCompleted { get; set; } = new List<string>();
        public string CurrentChapter { get; set; }
        public List<EventProgress> EventsCompleted { get; set; } = new List<EventProgress>();
    }

    public class EventProgress
    {
        public string ChapterId { get; set; }
        public string EventId { get; set; }
        public List<string> Choices { get; set; } = new List<string>();
        public List<string> Outcomes { get; set; } = new List<string>();
    }
}
