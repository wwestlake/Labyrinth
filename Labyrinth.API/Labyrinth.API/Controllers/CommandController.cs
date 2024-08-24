using Labyrinth.API.Common;
using Labyrinth.API.Entities;
using Labyrinth.API.Services;
using Labyrinth.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labyrinth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Moderator,Administrator,Owner")]
    public class CommandController : ControllerBase
    {
        private readonly ICommandProcessor _commandProcessor;

        public CommandController(ICommandProcessor commandProcessor)
        {
            _commandProcessor = commandProcessor;
        }

        [HttpPost]
        [Route("execute-command")]
        public IActionResult ExecuteCommand([FromBody] CommandAst command)
        {
            if (command == null)
            {
                return BadRequest("Command cannot be null.");
            }

            // Simulate fetching the current user (in a real app, you would get this from the JWT token or user context)
            var user = GetUserContext(); // This should return an ApplicationUser object
            var result = _commandProcessor.ProcessCommand(command, user);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Errors[0].Message);
            }
        }

        private ApplicationUser GetUserContext()
        {
            // Retrieve the user context from the current request or user session
            // This is just a placeholder implementation
            return new ApplicationUser
            {
                UserId = "some-user-id",
                Role = Role.User,
                Email = "user@example.com",
                DisplayName = "Example User"
            };
        }
    }
}
