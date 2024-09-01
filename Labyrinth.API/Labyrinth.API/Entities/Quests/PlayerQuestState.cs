using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Quests
{
    public class PlayerQuestState
    {
        [BsonId]
        public ObjectId Id { get; set; }  // MongoDB automatically assigns an ObjectId

        [BsonElement("questId")]
        public string QuestId { get; set; }

        [BsonElement("characterId")]
        public ObjectId CharacterId { get; set; } // Reference to the PlayerCharacter's ObjectId

        [BsonElement("currentObjectives")]
        public List<QuestObjective> CurrentObjectives { get; set; } = new List<QuestObjective>();

        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; }

        [BsonElement("isFailed")]
        public bool IsFailed { get; set; }
    }
}
