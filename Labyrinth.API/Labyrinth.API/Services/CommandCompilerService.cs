using FluentResults;
using Labyrinth.Common;
using System;

namespace Labyrinth.API.Services
{
    public class CommandCompilerService : ICommandCompilerService
    {
        private readonly Func<string, Result<CommandAst>> _compileCommand;

        // Inject the F# compileCommand function via constructor
        public CommandCompilerService(Func<string, Result<CommandAst>> compileCommand)
        {
            _compileCommand = compileCommand;
        }

        public Result<CommandAst> CompileCommand(string input)
        {
            // Call the F# function to compile the command
            var result = _compileCommand(input);

            if (result.IsSuccess)
            {
                Console.WriteLine("Command successfully compiled.");
            }
            else
            {
                Console.WriteLine($"Failed to compile command: {result.Errors[0].Message}");
            }

            return result;
        }
    }
}
