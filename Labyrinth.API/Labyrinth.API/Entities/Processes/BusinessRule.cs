using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Processes
{
    // Represents a business rule within a process
    public class BusinessRule
    {
        [BsonElement("ruleId")]
        public ObjectId RuleId { get; set; } = ObjectId.GenerateNewId();  // Unique identifier for the business rule

        [BsonElement("description")]
        public string RuleDescription { get; set; }  // Description of the rule

        [BsonElement("criteria")]
        public string Criteria { get; set; }  // The criteria that must be met for the rule to apply

        [BsonElement("action")]
        public string Action { get; set; }  // The action to take if the rule is triggered
    }
}
