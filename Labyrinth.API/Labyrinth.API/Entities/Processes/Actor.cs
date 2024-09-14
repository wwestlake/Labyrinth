using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Labyrinth.API.Entities.Processes
{
    // Represents an actor involved in the process
    public class Actor
    {
        [BsonElement("actorId")]
        public ObjectId ActorId { get; set; } = ObjectId.GenerateNewId();  // Unique identifier for each actor

        [BsonElement("role")]
        public string Role { get; set; }  // The role of the actor (e.g., "Admin", "User")

        [BsonElement("stepId")]
        public ObjectId StepId { get; set; }  // Foreign key linking to a step, if specific to a step
    }
}
