using Labyrinth.API.Common;
using Labyrinth.API.Dto;
using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Services;
using Labyrinth.API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Labyrinth.API.Controllers
{
    [Authorize] // Ensure that only authorized users can access these endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        // POST: api/Character/Generate
        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateCharacter([FromBody] CharacterCreationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is missing.");
            }

            var character = await _characterService.CreateCharacterAsync(request.Name, request.CharacterClass, request.Race);
            character.Owner = userId; // Assign the character to the current user

            return Ok(character);
        }

        // PUT: api/Character/Update/{id}
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateCharacter(string id, [FromBody] CharacterUpdateRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is missing.");
            }

            var character = await _characterService.GetCharacterByIdAsync(id);

            if (character == null)
            {
                return NotFound("Character not found.");
            }

            // Check if the user is the owner or has Admin/Owner role
            if (character.Owner != userId && userRole != Role.Administrator.ToString() && userRole != Role.Owner.ToString())
            {
                return Forbid("You do not have permission to modify this character.");
            }

            // Only Admins and Owners can modify anything, others can only spend points or change name/description
            if (character.Owner == userId && userRole != Role.Administrator.ToString() && userRole != Role.Owner.ToString())
            {
                // Check if the submitted stats are only modifying allowed fields
                if (request.UpdatedStats.Keys.Except(new[] { "Name", "Description", "AvailablePoints" }).Any())
                {
                    return BadRequest("You are only allowed to change the name, description, or spend earned points.");
                }
            }

            if (CharacterValidator.ValidateCharacterStats(character, request.UpdatedStats))
            {
                UpdateCharacterStats(character, request.UpdatedStats);
                await _characterService.UpdateCharacterAsync(id, character);
                return Ok("Character updated successfully.");
            }
            else
            {
                NotifyAdminsOfFraudAttempt(character, request.UpdatedStats);
                return BadRequest("Invalid character stats. Possible fraud attempt detected.");
            }
        }

        // PATCH: api/Character/UpdateNameDescription/{id}
        [HttpPatch("UpdateNameDescription/{id}")]
        public async Task<IActionResult> UpdateNameAndDescription(string id, [FromBody] CharacterNameDescriptionUpdateRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is missing.");
            }

            var character = await _characterService.GetCharacterByIdAsync(id);

            if (character == null)
            {
                return NotFound("Character not found.");
            }

            // Check if the user is the owner or has Admin/Owner role
            if (character.Owner != userId && userRole != Role.Administrator.ToString() && userRole != Role.Owner.ToString())
            {
                return Forbid("You do not have permission to modify this character.");
            }

            // Update name and description
            character.Name = request.Name;
            character.Description = request.Description;

            await _characterService.UpdateCharacterAsync(id, character);

            return Ok("Character name and description updated successfully.");
        }

        // Method to update character stats with submitted values
        private void UpdateCharacterStats(Character character, IDictionary<string, int> submittedStats)
        {
            if (submittedStats.TryGetValue("Health", out var healthValue))
            {
                character.Health.BaseValue = healthValue;
            }

            if (submittedStats.TryGetValue("Mana", out var manaValue))
            {
                character.Mana.BaseValue = manaValue;
            }

            if (submittedStats.TryGetValue("Strength", out var strengthValue))
            {
                character.Strength.BaseValue = strengthValue;
            }

            if (submittedStats.TryGetValue("Dexterity", out var dexterityValue))
            {
                character.Dexterity.BaseValue = dexterityValue;
            }

            if (submittedStats.TryGetValue("Constitution", out var constitutionValue))
            {
                character.Constitution.BaseValue = constitutionValue;
            }

            if (submittedStats.TryGetValue("Intelligence", out var intelligenceValue))
            {
                character.Intelligence.BaseValue = intelligenceValue;
            }

            if (submittedStats.TryGetValue("Wisdom", out var wisdomValue))
            {
                character.Wisdom.BaseValue = wisdomValue;
            }

            if (submittedStats.TryGetValue("Charisma", out var charismaValue))
            {
                character.Charisma.BaseValue = charismaValue;
            }

            if (submittedStats.TryGetValue("Luck", out var luckValue))
            {
                character.Luck.BaseValue = luckValue;
            }
        }

        // Method to notify admins of a potential fraud attempt
        private void NotifyAdminsOfFraudAttempt(Character character, IDictionary<string, int> submittedStats)
        {
            // Implement logic to notify admins, e.g., sending an email or logging the event
        }

        // In CharacterController.cs
        [HttpGet("check-name-unique")]
        public async Task<IActionResult> CheckNameUnique([FromQuery] string name)
        {
            var isUnique = await _characterService.IsCharacterNameUniqueAsync(name);
            return Ok(new { isUnique });
        }
    }
}
