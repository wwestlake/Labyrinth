using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items
{
    public class Container : Item
    {
        [BsonElement("capacity")]
        public int Capacity { get; set; } // Max number of items

        [BsonElement("weightLimit")]
        public int WeightLimit { get; set; } // Max weight it can carry

        [BsonElement("contents")]
        public List<Item> Contents { get; set; } = new List<Item>();
        public bool IsLocked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RequiredKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Container(string name, string description, int capacity, int weightLimit)
            : base(ItemType.Container, name, description)
        {
            Capacity = capacity;
            WeightLimit = weightLimit;
        }

        public void Open()
        {
            // Implement open logic
        }

        public void Store(Item item)
        {
            // Implement store logic
        }

        public void Remove(Item item)
        {
            // Implement remove logic
        }

        public bool Open(string key)
        {
            throw new NotImplementedException();
        }
    }
}
