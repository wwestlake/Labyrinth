using FluentResults;
using Labyrinth.Common;
using Labyrinth.API.Entities;
using Microsoft.Extensions.Logging;
using RulesEngine.Models;

namespace Labyrinth.API.Services
{
    public class RulesEngineService : IRulesEngineService
    {
        private readonly RulesEngine.RulesEngine _rulesEngine;
        private readonly ILogger<RulesEngineService> _logger;

        public RulesEngineService(RulesEngine.RulesEngine rulesEngine, ILogger<RulesEngineService> logger)
        {
            _rulesEngine = rulesEngine;
            _logger = logger;
        }

        public Result<CommandAst> EvaluateRules(CommandAst command, ApplicationUser user)
        {
            _logger.LogInformation("Evaluating command against rules...");

            var inputs = new[]
            {
                new RuleParameter("command", command),
                new RuleParameter("user", user)
            };

            var ruleResults = _rulesEngine.ExecuteAllRulesAsync("CommandRules", inputs).Result;

            foreach (var result in ruleResults)
            {
                if (!result.IsSuccess)
                {
                    _logger.LogWarning($"Rule {result.Rule.RuleName} failed: {result.ExceptionMessage}");
                    return Result.Fail(result.ExceptionMessage);
                }
            }

            return Result.Ok(command);
        }
    }
}
