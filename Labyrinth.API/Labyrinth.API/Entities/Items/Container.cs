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

        public bool Open(string key)
        {
            throw new NotImplementedException();
        }
    }
}
