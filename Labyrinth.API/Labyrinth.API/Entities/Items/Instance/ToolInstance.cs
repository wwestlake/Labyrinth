using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public class ToolInstance : ItemInstance
    {
        [BsonElement("toolType")]
        public string ToolType { get; set; } // Type of the tool, e.g., Pickaxe, Fishing Rod

        [BsonElement("currentDurability")]
        public int CurrentDurability { get; set; } // Current durability of the tool

        [BsonElement("currentEfficiency")]
        public int CurrentEfficiency { get; set; } // Current efficiency of the tool

        public ToolInstance(ObjectId prototypeId, string ownerId, string toolType, int initialDurability, int initialEfficiency)
            : base(prototypeId, ownerId)
        {
            ToolType = toolType;
            CurrentDurability = initialDurability;
            CurrentEfficiency = initialEfficiency;
        }

        // Method to use the tool, decreasing its durability
        public void Use()
        {
            if (CurrentDurability > 0)
            {
                CurrentDurability--;
                // Implement additional use logic here, such as affecting game state or triggering events
            }
        }

        // Method to repair the tool, restoring its durability
        public void Repair(int repairAmount)
        {
            CurrentDurability += repairAmount;
            // Optionally, implement a maximum durability check based on the prototype or game rules
        }

        // Method to upgrade the tool, increasing its efficiency
        public void Upgrade(int efficiencyIncrease)
        {
            CurrentEfficiency += efficiencyIncrease;
            // Implement additional upgrade logic, such as enhancing tool capabilities or unlocking new features
        }

        // Method to reset the tool's durability (e.g., if broken or fully repaired)
        public void ResetDurability(int maxDurability)
        {
            CurrentDurability = maxDurability;
        }

        // Method to reset the tool's efficiency
        public void ResetEfficiency(int baseEfficiency)
        {
            CurrentEfficiency = baseEfficiency;
        }
    }
}
