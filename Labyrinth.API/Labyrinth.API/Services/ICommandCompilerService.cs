using FluentResults;
using Labyrinth.Common;
using System;

namespace Labyrinth.API.Services;

public interface ICommandCompilerService
{
    Result<CommandAst> CompileCommand(string input);
}
