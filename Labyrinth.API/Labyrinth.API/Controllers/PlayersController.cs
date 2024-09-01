using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Labyrinth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // This ensures that all endpoints require an authenticated user
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IUserService _userService; // Assuming this service helps fetch user details

        public PlayersController(IPlayerService playerService, IUserService userService)
        {
            _playerService = playerService;
            _userService = userService;
        }

        // GET: api/players/{characterId}
        [HttpGet("{characterId}")]
        public async Task<IActionResult> GetPlayer(string characterId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from JWT token

            if (userId == null)
                return Unauthorized("User ID not found.");

            var player = await _playerService.GetPlayerAsync(userId, characterId);
            if (player == null)
                return NotFound($"Player with Character ID {characterId} not found for the current user.");

            return Ok(player);
        }

        // POST: api/players
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerCharacter character)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from JWT token

            if (userId == null)
                return Unauthorized("User ID not found.");

            var user = await _userService.GetUserByUidAsync(userId);
            if (user == null)
                return NotFound($"User with ID {userId} not found.");

            var player = await _playerService.CreatePlayerAsync(user, character);

            return CreatedAtAction(nameof(GetPlayer), new { characterId = player.CharacterId }, player);
        }

        // PUT: api/players/{characterId}
        [HttpPut("{characterId}")]
        public async Task<IActionResult> UpdatePlayer(string characterId, [FromBody] Player updatedPlayer)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from JWT token

            if (userId == null)
                return Unauthorized("User ID not found.");

            var existingPlayer = await _playerService.GetPlayerAsync(userId, characterId);
            if (existingPlayer == null)
                return NotFound($"Player with Character ID {characterId} not found for the current user.");

            updatedPlayer.Id = existingPlayer.Id; // Ensure we're updating the correct document
            await _playerService.UpdatePlayerAsync(updatedPlayer);

            return NoContent();
        }

        // DELETE: api/players/{characterId}
        [HttpDelete("{characterId}")]
        public async Task<IActionResult> DeletePlayer(string characterId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from JWT token

            if (userId == null)
                return Unauthorized("User ID not found.");

            var player = await _playerService.GetPlayerAsync(userId, characterId);
            if (player == null)
                return NotFound($"Player with Character ID {characterId} not found for the current user.");

            await _playerService.DeletePlayerAsync(player.Id);

            return NoContent();
        }

        // Additional endpoints for managing player state (e.g., set online/offline) can be added here
    }
}
