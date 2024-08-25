using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public class ConsumableInstance : ItemInstance
    {
        [BsonElement("currentQuantity")]
        public int CurrentQuantity { get; set; } // Current quantity of the consumable item

        [BsonElement("isActive")]
        public bool IsActive { get; set; } // Indicates if the consumable's effect is currently active

        [BsonElement("remainingDuration")]
        public int? RemainingDuration { get; set; } // Remaining duration of the active effect, if applicable

        public ConsumableInstance(ObjectId prototypeId, string ownerId, int initialQuantity)
            : base(prototypeId, ownerId)
        {
            CurrentQuantity = initialQuantity;
            IsActive = false;
            RemainingDuration = null;
        }

        // Method to use a consumable item
        public void UseConsumable()
        {
            if (CurrentQuantity > 0)
            {
                CurrentQuantity--;
                IsActive = true;
                // Assuming this uses the duration from the prototype; set RemainingDuration here
                // RemainingDuration = [duration from prototype]; (Replace with actual logic to fetch prototype details)
            }
        }

        // Method to reduce remaining duration (e.g., per game tick or turn)
        public void DecreaseDuration(int amount)
        {
            if (RemainingDuration.HasValue && RemainingDuration.Value > 0)
            {
                RemainingDuration -= amount;
                if (RemainingDuration <= 0)
                {
                    RemainingDuration = 0;
                    IsActive = false; // Effect is no longer active when duration ends
                }
            }
        }

        // Method to discard the consumable item
        public void Discard()
        {
            CurrentQuantity = 0;
            IsActive = false;
            RemainingDuration = null;
        }
    }
}
