using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Labyrinth.API.Entities.Processes
{
    // Represents an entire process document
    public class Process
    {
        [BsonId]
        public ObjectId Id { get; set; }  // Unique identifier for the process

        [BsonElement("name")]
        public string Name { get; set; }  // Name of the process

        [BsonElement("status")]
        public ProcessStatus Status { get; set; }  // Current status of the process

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }  // Timestamp when the process was initiated

        [BsonElement("modifiedDate")]
        public DateTime ModifiedDate { get; set; }  // Timestamp for the last update to the process

        [BsonElement("steps")]
        public List<ProcessStep> Steps { get; set; } = new List<ProcessStep>();  // List of steps in the process

        [BsonElement("businessRules")]
        public List<BusinessRule> BusinessRules { get; set; } = new List<BusinessRule>();  // List of business rules

        [BsonElement("actors")]
        public List<Actor> Actors { get; set; } = new List<Actor>();  // List of actors involved in the process
    }
}
