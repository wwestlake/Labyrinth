using FluentResults;
using Labyrinth.API.Entities;
using Labyrinth.Common;

namespace Labyrinth.API.Services;

public interface IRulesEngineService
{
    Result<CommandAst> EvaluateRules(CommandAst command, ApplicationUser user);
}
