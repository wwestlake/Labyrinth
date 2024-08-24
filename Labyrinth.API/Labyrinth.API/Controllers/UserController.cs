using Labyrinth.API.Common;
using Labyrinth.API.Entities;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace Labyrinth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User/me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApplicationUser>> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID not found in token.");
            }

            var user = await _userService.GetUserByUidAsync(userIdClaim);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        // GET: api/User/{uid}
        [HttpGet("{uid}")]
        [Authorize(Policy = "CanViewAllUsers")]
        public async Task<ActionResult<ApplicationUser>> GetUserByUid(string uid)
        {
            var user = await _userService.GetUserByUidAsync(uid);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Policy = "CanViewAllUsers")]
        public async Task<ActionResult<List<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // POST: api/User
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] ApplicationUser user)
        {
            // Check if there are any users in the database
            var allUsers = await _userService.GetAllUsersAsync();

            if (allUsers.Count == 0)
            {
                // This is the first user, assign Owner role
                user.Role = Role.Owner;
            }
            else
            {
                // For all subsequent users, assign User role by default
                user.Role = Role.User;
            }

            if (string.IsNullOrEmpty(user.Id))
            {
                user.Id = ObjectId.GenerateNewId().ToString();
            }

            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserByUid), new { uid = createdUser.UserId }, createdUser);
        }

        // PUT: api/User/me
        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] ApplicationUser updatedUser)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("User ID not found in token.");
            }

            if (userIdClaim != updatedUser.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                await _userService.UpdateUserAsync(userIdClaim, updatedUser);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/User/{uid}
        [HttpPut("{uid}")]
        [Authorize(Policy = "CanModifyAnyUser")]
        public async Task<IActionResult> UpdateUserByUid(string uid, [FromBody] ApplicationUser updatedUser)
        {
            if (uid != updatedUser.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                await _userService.UpdateUserAsync(uid, updatedUser);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/User/{uid}
        [HttpDelete("{uid}")]
        [Authorize(Policy = "CanBanUsers")]
        public async Task<IActionResult> DeleteUser(string uid)
        {
            try
            {
                await _userService.DeleteUserAsync(uid);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/User/{uid}/role
        [HttpPost("{uid}/role")]
        [Authorize(Policy = "CanModifyAnyUser")]
        public async Task<IActionResult> AssignUserRole(string uid, [FromBody] Role role)
        {
            try
            {
                await _userService.AssignUserRole(uid, role);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
