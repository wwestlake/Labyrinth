using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Entities.Quests;
using System.Collections.Generic;

namespace Labyrinth.API.Services
{
    public class PlayerQuestLog
    {
        private readonly QuestManager _questManager;

        public PlayerQuestLog(QuestManager questManager)
        {
            _questManager = questManager;
        }

        public void DisplayActiveQuests(PlayerCharacter character)
        {
            var activeQuests = _questManager.GetActiveQuests(character);
            // Logic to display active quests to the player
        }

        public void DisplayCompletedQuests(PlayerCharacter character)
        {
            var completedQuests = character.CompletedQuests;
            // Logic to display completed quests to the player
        }

        public void DisplayQuestDetails(PlayerCharacter character, string questId)
        {
            var questState = character.ActiveQuests.FirstOrDefault(q => q.QuestId == questId);
            // Logic to display quest details to the player
        }
    }
}
