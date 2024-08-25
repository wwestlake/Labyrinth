namespace Labyrinth.API.Services;

using Labyrinth.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RoomService
{
    private readonly IMongoCollection<Room> _rooms;

    public RoomService(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _rooms = database.GetCollection<Room>(collectionName);
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _rooms.Find(room => true).ToListAsync();
    }

    public async Task<Room> GetRoomByIdAsync(ObjectId id)
    {
        return await _rooms.Find<Room>(room => room.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Room> CreateRoomAsync(Room room)
    {
        await _rooms.InsertOneAsync(room);
        return room;
    }

    public async Task UpdateRoomAsync(ObjectId id, Room updatedRoom)
    {
        await _rooms.ReplaceOneAsync(room => room.Id == id, updatedRoom);
    }

    public async Task DeleteRoomAsync(ObjectId id)
    {
        await _rooms.DeleteOneAsync(room => room.Id == id);
    }
}
