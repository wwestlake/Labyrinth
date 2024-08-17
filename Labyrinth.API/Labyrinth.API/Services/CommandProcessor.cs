using FluentResults;
using Microsoft.Extensions.Logging;
using Labyrinth.Common;
using Labyrinth.API.Entities;

namespace Labyrinth.API.Services
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IRulesEngineService _rulesEngineService;
        private readonly ILogger<CommandProcessor> _logger;

        public CommandProcessor(IRulesEngineService rulesEngineService, ILogger<CommandProcessor> logger)
        {
            _rulesEngineService = rulesEngineService;
            _logger = logger;
        }

        public Result<CommandAst> ProcessCommand(CommandAst command, ApplicationUser user)
        {
            _logger.LogInformation($"Processing command: {command.GetType().Name}");

            // Step 1: Validate the command (basic checks)
            var validationResult = ValidateCommand(command, user);
            if (validationResult.IsFailed)
            {
                return validationResult;
            }

            // Step 2: Evaluate the command against rules
            var ruleResult = _rulesEngineService.EvaluateRules(command, user);
            if (ruleResult.IsFailed)
            {
                return ruleResult;
            }

            // Step 3: Execute the command
            return ExecuteCommand(command, user);
        }

        private Result<CommandAst> ValidateCommand(CommandAst command, ApplicationUser user)
        {
            // Add basic validation logic here
            _logger.LogInformation("Validating command...");
            return Result.Ok(command);
        }

        private Result<CommandAst> ExecuteCommand(CommandAst command, ApplicationUser user)
        {
            // Execute the command here
            _logger.LogInformation($"Executing command: {command.GetType().Name}");
            return Result.Ok(command);
        }
    }
}
