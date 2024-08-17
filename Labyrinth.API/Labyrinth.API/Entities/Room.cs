namespace Labyrinth.API.Entities;

using System.Collections.Generic;
using Labyrinth.API.Entities.Items;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Room
{
    [BsonId]
    public ObjectId Id { get; set; }  // MongoDB automatically assigns an ObjectId

    [BsonElement("name")]
    public string Name { get; set; }  // Room name

    [BsonElement("description")]
    public string Description { get; set; }  // Room description

    [BsonElement("exits")]
    public List<Exit> Exits { get; set; } = new List<Exit>();  // List of exits to other rooms

    [BsonElement("items")]
    public List<Item> Items { get; set; } = new List<Item>();  // List of items in the room
}
