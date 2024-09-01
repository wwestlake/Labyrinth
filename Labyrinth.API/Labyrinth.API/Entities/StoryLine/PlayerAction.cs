using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Labyrinth.API.Entities.Storyline
{
    public class PlayerAction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PlayerId { get; set; }
        public string ChapterId { get; set; }
        public string EventId { get; set; }
        public string Choice { get; set; }
        public string Outcome { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
