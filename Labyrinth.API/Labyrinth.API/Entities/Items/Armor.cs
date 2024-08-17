using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items
{
    public class Armor : Item
    {
        [BsonId]
        public ObjectId Id { get; set; }  // MongoDB automatically assigns an ObjectId

        [BsonElement("defense")]
        public int Defense { get; set; }

        [BsonElement("armorType")]
        public string ArmorType { get; set; } // E.g., Helmet, Chestplate

        [BsonElement("durability")]
        public int Durability { get; set; }

        [BsonElement("weight")]
        public int Weight { get; set; }

        [BsonElement("elementalResistance")]
        public string ElementalResistance { get; set; } // E.g., fire, ice

        public Armor(string name, string description, int defense, string armorType, int durability, int weight, string elementalResistance)
            : base(ItemType.Armor, name, description)
        {
            Defense = defense;
            ArmorType = armorType;
            Durability = durability;
            Weight = weight;
            ElementalResistance = elementalResistance;
        }

        public void Equip()
        {
            // Implement equip logic
        }

        public void Repair()
        {
            // Implement repair logic
        }
    }
}
