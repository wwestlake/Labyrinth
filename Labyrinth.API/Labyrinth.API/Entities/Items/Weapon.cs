using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items
{
    public class Weapon : Item
    {
        [BsonElement("damage")]
        public int Damage { get; set; }

        [BsonElement("range")]
        public string Range { get; set; } // E.g., Melee, Ranged

        [BsonElement("durability")]
        public int Durability { get; set; }

        [BsonElement("criticalChance")]
        public double CriticalChance { get; set; }

        [BsonElement("speed")]
        public string Speed { get; set; } // E.g., slow, medium, fast

        public Weapon(string name, string description, int damage, string range, int durability, double criticalChance, string speed)
            : base(ItemType.Weapon, name, description)
        {
            Damage = damage;
            Range = range;
            Durability = durability;
            CriticalChance = criticalChance;
            Speed = speed;
        }

        public void Attack()
        {
            // Implement attack logic
        }

        public void Repair()
        {
            // Implement repair logic
        }
    }
}
