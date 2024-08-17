using Labyrinth.Common;
using FluentResults;
using Labyrinth.API.Entities;  // Make sure this namespace includes ApplicationUser

namespace Labyrinth.API.Services
{
    public interface ICommandProcessor
    {
        Result<CommandAst> ProcessCommand(CommandAst command, ApplicationUser user);
    }
}
