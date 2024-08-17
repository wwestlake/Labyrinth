using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items
{
    public class Tool : Item
    {
        [BsonElement("toolType")]
        public string ToolType { get; set; } // E.g., Pickaxe, Fishing Rod

        [BsonElement("durability")]
        public int Durability { get; set; }

        [BsonElement("efficiency")]
        public int Efficiency { get; set; }

        public Tool(string name, string description, string toolType, int durability, int efficiency)
            : base(ItemType.Tool, name, description)
        {
            ToolType = toolType;
            Durability = durability;
            Efficiency = efficiency;
        }

        public void Use()
        {
            // Implement use logic
        }

        public void Repair()
        {
            // Implement repair logic
        }

        public void Upgrade()
        {
            // Implement upgrade logic
        }
    }
}
