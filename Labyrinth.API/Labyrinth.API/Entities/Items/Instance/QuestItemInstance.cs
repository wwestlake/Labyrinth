using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public class QuestItemInstance : ItemInstance
    {
        [BsonElement("questId")]
        public string QuestId { get; set; }  // ID of the quest this item is associated with

        [BsonElement("isActivated")]
        public bool IsActivated { get; set; }  // Indicates if the quest item has been activated

        [BsonElement("isCombined")]
        public bool IsCombined { get; set; }  // Indicates if the quest item has been combined with another item

        [BsonElement("isUniqueInstance")]
        public bool IsUniqueInstance { get; set; }  // Indicates if this item instance is unique within the game world

        public QuestItemInstance(ObjectId prototypeId, string ownerId, string questId, bool isUniqueInstance)
            : base(prototypeId, ownerId)
        {
            QuestId = questId;
            IsUniqueInstance = isUniqueInstance;
            IsActivated = false;
            IsCombined = false;
        }

        // Method to activate the quest item
        public void Activate()
        {
            if (!IsActivated)
            {
                IsActivated = true;
                // Implement additional activation logic here
            }
        }

        // Method to combine the quest item with another item
        public void Combine()
        {
            if (!IsCombined)
            {
                IsCombined = true;
                // Implement combination logic here
            }
        }

        // Method to reset the quest item instance (e.g., if a quest is restarted)
        public void Reset()
        {
            IsActivated = false;
            IsCombined = false;
            // Implement additional reset logic if necessary
        }
    }
}
