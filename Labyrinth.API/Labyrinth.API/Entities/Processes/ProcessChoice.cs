using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Labyrinth.API.Entities.Processes
{
    // Represents a choice within a process step
    public class ProcessChoice
    {
        [BsonElement("choiceId")]
        public ObjectId ChoiceId { get; set; } = ObjectId.GenerateNewId();  // Unique identifier for each choice

        [BsonElement("condition")]
        public string Condition { get; set; }  // Condition that triggers this choice

        [BsonElement("nextStepId")]
        public ObjectId NextStepId { get; set; }  // The step to go to if this choice is made
    }
}
