using Labyrinth.API.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Labyrinth.API.Entities
{
    public class MetaData<T>
    {
        [BsonId]
        [JsonConverter(typeof(ObjectIdJsonConverter))] // Custom converter to handle ObjectId
        public ObjectId Id { get; set; }

        public string TargetId { get; set; }  // Reference to the target object
        public T Payload { get; set; }  // Generic payload data associated with the target

        public MetaData(string targetId, T payload)
        {
            TargetId = targetId;
            Payload = payload;
        }
    }
}
