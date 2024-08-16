using Labyrinth.API.Common;
using Labyrinth.API.Entities;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // GET: api/User/{uid}
        [HttpGet("{uid}")]
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
        public async Task<ActionResult<List<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> CreateUser([FromBody] ApplicationUser user)
        {
            // Optional: Add validation logic for creating users

            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserByUid), new { uid = createdUser.UserId }, createdUser);
        }

        // PUT: api/User/{uid}
        [HttpPut("{uid}")]
        public async Task<IActionResult> UpdateUser(string uid, [FromBody] ApplicationUser updatedUser)
        {
            if (uid != updatedUser.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                await _userService.UpdateUserAsync(uid, updatedUser);
                return NoContent(); // HTTP 204
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/User/{uid}
        [HttpDelete("{uid}")]
        public async Task<IActionResult> DeleteUser(string uid)
        {
            try
            {
                await _userService.DeleteUserAsync(uid);
                return NoContent(); // HTTP 204
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/User/{uid}/role
        [HttpPost("{uid}/role")]
        public async Task<IActionResult> AssignUserRole(string uid, [FromBody] Role role)
        {
            try
            {
                await _userService.AssignUserRole(uid, role);
                return NoContent(); // HTTP 204
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
