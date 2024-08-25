using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Entities.Quests;

namespace Labyrinth.API.Services
{
    public interface IQuestEngine
    {
        void EvaluateConditions(PlayerCharacter character, string questId, PlayerQuestState questState, object progressData);
    }

    public class QuestEngine : IQuestEngine
    {
        public void EvaluateConditions(PlayerCharacter character, string questId, PlayerQuestState questState, object progressData)
        {
            // Evaluate conditions based on progressData and update questState accordingly
            foreach (var objective in questState.CurrentObjectives)
            {
                // Check conditions and set objective.IsCompleted to true if conditions are met
            }
        }
    }
}
