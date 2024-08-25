using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public class TreasureInstance : ItemInstance
    {
        [BsonElement("currentValue")]
        public int CurrentValue { get; set; }  // Current market value of the treasure item

        [BsonElement("rarity")]
        public string Rarity { get; set; }  // Rarity level, e.g., Common, Rare

        [BsonElement("weight")]
        public int Weight { get; set; }  // Weight of the treasure item

        [BsonElement("isInspected")]
        public bool IsInspected { get; set; }  // Whether the treasure has been inspected

        public TreasureInstance(ObjectId prototypeId, string ownerId, int initialValue, string rarity, int weight)
            : base(prototypeId, ownerId)
        {
            CurrentValue = initialValue;
            Rarity = rarity;
            Weight = weight;
            IsInspected = false;
        }

        // Method to sell the treasure item
        public void Sell()
        {
            // Implement sell logic here
            // For example, remove the item from inventory and add value to player's currency
        }

        // Method to inspect the treasure item, potentially revealing additional details or value
        public void Inspect()
        {
            if (!IsInspected)
            {
                IsInspected = true;
                // Implement inspection logic here, such as identifying the treasure or revealing hidden details
            }
        }

        // Method to adjust the value of the treasure, possibly due to market changes or rarity fluctuations
        public void AdjustValue(int newValue)
        {
            CurrentValue = newValue;
        }

        // Method to reset the treasure's inspected status
        public void ResetInspection()
        {
            IsInspected = false;
        }
    }
}
