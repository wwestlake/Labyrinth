namespace Labyrinth.API.Entities;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Exit
{
    [BsonElement("direction")]
    public string Direction { get; set; }  // E.g., "North", "South", "East", "West"

    [BsonElement("roomId")]
    public ObjectId RoomId { get; set; }  // ID of the room this exit leads to
}
