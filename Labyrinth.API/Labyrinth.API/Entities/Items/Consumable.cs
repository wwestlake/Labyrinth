using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items
{
    public class Consumable : Item
    {
        [BsonElement("effect")]
        public string Effect { get; set; } // E.g., Healing, Buff

        [BsonElement("duration")]
        public int Duration { get; set; } // Duration in seconds

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        public Consumable(string name, string description, string effect, int duration, int quantity)
            : base(ItemType.Consumable, name, description)
        {
            Effect = effect;
            Duration = duration;
            Quantity = quantity;
        }

        public void Use()
        {
            // Implement use logic
        }

        public void Discard()
        {
            // Implement discard logic
        }
    }
}
