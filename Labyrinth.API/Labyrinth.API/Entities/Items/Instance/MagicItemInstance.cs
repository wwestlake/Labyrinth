using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public class MagicItemInstance : ItemInstance
    {
        [BsonElement("currentManaCost")]
        public int CurrentManaCost { get; set; } // Current mana cost of the magic item

        [BsonElement("currentCooldown")]
        public int CurrentCooldown { get; set; } // Current cooldown time left in seconds

        [BsonElement("isOnCooldown")]
        public bool IsOnCooldown { get; set; } // Whether the magic item is currently on cooldown

        public MagicItemInstance(ObjectId prototypeId, string ownerId)
            : base(prototypeId, ownerId)
        {
            CurrentManaCost = 0;
            CurrentCooldown = 0;
            IsOnCooldown = false;
        }

        // Method to cast the magic item, applying its effects and initiating cooldown
        public void Cast()
        {
            if (!IsOnCooldown)
            {
                // Implement the effect logic here based on the prototype's magic effect
                IsOnCooldown = true;
                // Assume this sets the cooldown based on the prototype; replace with actual logic to fetch prototype details
                // CurrentCooldown = [cooldown from prototype];
            }
        }

        // Method to reduce the cooldown timer, called periodically
        public void ReduceCooldown(int timeElapsed)
        {
            if (IsOnCooldown && CurrentCooldown > 0)
            {
                CurrentCooldown -= timeElapsed;
                if (CurrentCooldown <= 0)
                {
                    CurrentCooldown = 0;
                    IsOnCooldown = false; // Cooldown is over
                }
            }
        }

        // Method to disenchant the magic item
        public void Disenchant()
        {
            // Implement disenchant logic here
        }

        // Method to recharge the magic item (resetting cooldown and possibly restoring charges)
        public void Recharge()
        {
            IsOnCooldown = false;
            CurrentCooldown = 0;
            // Implement additional recharge logic if needed
        }
    }
}
