using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Labyrinth.API.Entities.Storyline
{
    public class StoryEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ChapterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<EventStage> Stages { get; set; } = new List<EventStage>();
        public List<string> Outcomes { get; set; } = new List<string>();
    }

    public class EventStage
    {
        public int Stage { get; set; }
        public string Description { get; set; }
        public List<EventChoice> Choices { get; set; } = new List<EventChoice>();
    }

    public class EventChoice
    {
        public string Choice { get; set; }
        public string Outcome { get; set; }
    }
}
