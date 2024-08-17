using Labyrinth.API.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Labyrinth.API.Entities.Items
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Consumable,
        Treasure,
        QuestItem,
        Tool,
        MagicItem,
        Container
    }

    public class Item
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdJsonConverter))]
        public ObjectId Id { get; set; }  // MongoDB automatically assigns an ObjectId

        [BsonElement("type")]
        public ItemType Type { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        public Item(ItemType type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;
        }
    }
}
