using Microsoft.AspNetCore.Mvc;
using Labyrinth.Common;
using Labyrinth.API.Services;
using FluentResults;
using Microsoft.AspNetCore.Authorization;

namespace Labyrinth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Moderator,Administrator,Owner")]
    public class CommandController : ControllerBase
    {
        private readonly ICommandCompilerService _compilerService;

        public CommandController(ICommandCompilerService compilerService)
        {
            _compilerService = compilerService;
        }

        [HttpPost]
        [Route("compile-command")]
        public IActionResult CompileCommand([FromBody] string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return BadRequest("Command cannot be empty.");
            }

            var result = _compilerService.CompileCommand(command);

            if (result.IsSuccess)
            {
                // Further process the CommandAst or return it
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Errors[0].Message);
            }
        }
    }
}
