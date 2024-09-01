using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public class ContainerInstance : ItemInstance
    {
        [BsonElement("currentCapacity")]
        public int CurrentCapacity { get; set; } // Current number of items in the container

        [BsonElement("currentWeight")]
        public int CurrentWeight { get; set; } // Current weight of the items in the container

        [BsonElement("weightLimit")]
        public int WeightLimit { get; set; } // Max weight the container can carry

        [BsonElement("contents")]
        public List<ItemInstance> Contents { get; set; } = new List<ItemInstance>(); // List of item instances in the container

        [BsonElement("isLocked")]
        public bool IsLocked { get; set; }  // Whether the container is currently locked

        [BsonElement("requiredKey")]
        public string RequiredKey { get; set; }  // The key required to open the container

        public ContainerInstance(ObjectId prototypeId, string ownerId, int initialCapacity, int initialWeight, int weightLimit, bool isLocked, string requiredKey)
            : base(prototypeId, ownerId)
        {
            CurrentCapacity = initialCapacity;
            CurrentWeight = initialWeight;
            WeightLimit = weightLimit;
            IsLocked = isLocked;
            RequiredKey = requiredKey;
        }

        // Method to open the container if it's locked
        public bool Open(string key)
        {
            if (IsLocked && key == RequiredKey)
            {
                IsLocked = false;
                return true; // Successfully opened
            }
            return !IsLocked; // Already open or incorrect key
        }

        // Method to store an item instance in the container
        public bool Store(ItemInstance item)
        {
            if (CurrentCapacity < Contents.Count + 1 && CurrentWeight + item.Weight <= WeightLimit)
            {
                Contents.Add(item);
                CurrentWeight += item.Weight;
                return true;
            }
            return false; // Not enough capacity or weight limit exceeded
        }

        // Method to remove an item instance from the container
        public bool Remove(ItemInstance item)
        {
            if (Contents.Contains(item))
            {
                Contents.Remove(item);
                CurrentWeight -= item.Weight;
                return true;
            }
            return false;
        }

        // Method to check if the container is full
        public bool IsFull()
        {
            return Contents.Count >= CurrentCapacity;
        }

        // Method to check if the container is overloaded
        public bool IsOverloaded()
        {
            return CurrentWeight > WeightLimit;
        }
    }
}
