using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items
{
    public class Treasure : Item
    {
        [BsonElement("value")]
        public int Value { get; set; }

        [BsonElement("rarity")]
        public string Rarity { get; set; } // E.g., Common, Rare

        [BsonElement("weight")]
        public int Weight { get; set; }

        public Treasure(string name, string description, int value, string rarity, int weight)
            : base(ItemType.Treasure, name, description)
        {
            Value = value;
            Rarity = rarity;
            Weight = weight;
        }

        public void Sell()
        {
            // Implement sell logic
        }

        public void Inspect()
        {
            // Implement inspect logic
        }
    }
}
