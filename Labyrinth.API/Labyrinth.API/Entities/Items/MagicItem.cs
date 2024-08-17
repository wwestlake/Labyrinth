using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items
{
    public class MagicItem : Item
    {
        [BsonElement("magicEffect")]
        public string MagicEffect { get; set; } // E.g., Invisibility, Teleportation

        [BsonElement("manaCost")]
        public int ManaCost { get; set; }

        [BsonElement("cooldown")]
        public int Cooldown { get; set; } // Cooldown in seconds

        public MagicItem(string name, string description, string magicEffect, int manaCost, int cooldown)
            : base(ItemType.MagicItem, name, description)
        {
            MagicEffect = magicEffect;
            ManaCost = manaCost;
            Cooldown = cooldown;
        }

        public void Cast()
        {
            // Implement cast logic
        }

        public void Recharge()
        {
            // Implement recharge logic
        }

        public void Disenchant()
        {
            // Implement disenchant logic
        }
    }
}
