using Labyrinth.API.Entities;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Labyrinth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(string id)
        {
            var objectId = new ObjectId(id);
            var room = await _roomService.GetRoomByIdAsync(objectId);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom(Room newRoom)
        {
            var room = await _roomService.CreateRoomAsync(newRoom);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id.ToString() }, room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(string id, Room updatedRoom)
        {
            var objectId = new ObjectId(id);
            var room = await _roomService.GetRoomByIdAsync(objectId);

            if (room == null)
            {
                return NotFound();
            }

            await _roomService.UpdateRoomAsync(objectId, updatedRoom);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            var objectId = new ObjectId(id);
            var room = await _roomService.GetRoomByIdAsync(objectId);

            if (room == null)
            {
                return NotFound();
            }

            await _roomService.DeleteRoomAsync(objectId);
            return NoContent();
        }
    }
}
