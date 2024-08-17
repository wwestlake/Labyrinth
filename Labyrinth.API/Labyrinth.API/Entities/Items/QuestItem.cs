using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items
{
    public class QuestItem : Item
    {
        [BsonElement("questId")]
        public string QuestId { get; set; }

        [BsonElement("isUnique")]
        public bool IsUnique { get; set; }

        public QuestItem(string name, string description, string questId, bool isUnique)
            : base(ItemType.QuestItem, name, description)
        {
            QuestId = questId;
            IsUnique = isUnique;
        }

        public void Activate()
        {
            // Implement activation logic
        }

        public void Combine()
        {
            // Implement combination logic
        }
    }
}
