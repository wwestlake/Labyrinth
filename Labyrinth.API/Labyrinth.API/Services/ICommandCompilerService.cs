using FluentResults;
using Labyrinth.Common;

namespace Labyrinth.API.Services;

public interface ICommandCompilerService
{
    Result<CommandAst> CompileCommand(string input);
}
