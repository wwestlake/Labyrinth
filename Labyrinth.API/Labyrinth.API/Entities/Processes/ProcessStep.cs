using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Processes
{
    // Represents a step within a process
    public class ProcessStep
    {
        [BsonElement("stepId")]
        public ObjectId StepId { get; set; } = ObjectId.GenerateNewId();  // Unique identifier for each step

        [BsonElement("description")]
        public string Description { get; set; }  // Description of the step

        [BsonElement("status")]
        public StepStatus Status { get; set; }  // Current status of the step

        [BsonElement("assignedTo")]
        public string AssignedTo { get; set; }  // User or role responsible for the step

        [BsonElement("outputData")]
        public string OutputData { get; set; }  // Data produced by the step, if any

        [BsonElement("choices")]
        public List<ProcessChoice> Choices { get; set; } = new List<ProcessChoice>();  // Choices available at this step
    }
}
