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

    public async Task SeedRoomsAsync()
    {
        var roomsExist = await _rooms.Find(_ => true).AnyAsync();
        if (!roomsExist)
        {
            var lobby = new Room
            {
                Name = "The Lobby",
                Description = "You stand in a large, dimly lit room. The atmosphere is charged with anticipation, as if countless adventurers have passed through here before setting off on their quests. The walls are adorned with ancient tapestries, and the floor is worn smooth from the footsteps of many. There's a sense of history and purpose in the air, and you know that beyond this room lies the unknown. Exits lead off in several directions, each promising its own set of challenges and rewards."
            };

            await _rooms.InsertOneAsync(lobby);
        }
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
