using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public class ArmorInstance : ItemInstance
    {
        [BsonElement("currentDefense")]
        public int? CurrentDefense { get; set; }  // Current defense value, modifiable

        public ArmorInstance(ObjectId prototypeId, string ownerId)
            : base(prototypeId, ownerId)
        {
        }

        // Method to adjust defense value
        public void AdjustDefense(int adjustment)
        {
            if (CurrentDefense.HasValue)
            {
                CurrentDefense += adjustment;
                if (CurrentDefense < 0)
                    CurrentDefense = 0;  // Ensure defense doesn't go negative
            }
        }
    }
}
