using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public abstract class ItemInstance
    {
        [BsonId]
        public ObjectId Id { get; set; }  // Unique identifier for the item instance

        [BsonElement("prototypeId")]
        public ObjectId PrototypeId { get; set; }  // Reference to the item's prototype

        [BsonElement("ownerId")]
        public string OwnerId { get; set; }  // ID of the owner of this item instance

        [BsonElement("weight")]
        public int Weight { get; set; }  // Weight of the item instance

        public ItemInstance(ObjectId prototypeId, string ownerId)
        {
            PrototypeId = prototypeId;
            OwnerId = ownerId;
            Weight = 0; // Default weight, should be set appropriately
        }
    }
}
