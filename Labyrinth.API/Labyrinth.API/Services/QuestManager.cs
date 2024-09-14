using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Entities.Quests;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth.API.Services
{
    public class QuestManager
    {
        private readonly IQuestDatabase _questDatabase;
        private readonly IQuestEngine _questEngine;

        public QuestManager(IQuestDatabase questDatabase, IQuestEngine questEngine)
        {
            _questDatabase = questDatabase;
            _questEngine = questEngine;
        }

        public void StartQuest(PlayerCharacter character, string questId)
        {
            var quest = _questDatabase.GetQuestDefinition(questId);
            if (quest != null && !CharacterHasQuest(character, questId))
            {
                var playerQuestState = new PlayerQuestState
                {
                    QuestId = questId,
                    CurrentObjectives = quest.Objectives.Select(o => new QuestObjective
                    {
                        Description = o.Description,
                        IsCompleted = false
                    }).ToList(),
                    IsCompleted = false,
                    IsFailed = false
                };

                character.ActiveQuests.Add(playerQuestState);
                _questDatabase.SaveQuestState(character, playerQuestState);
            }
        }

        public void ProgressQuest(PlayerCharacter character, string questId, object progressData)
        {
            var questState = character.ActiveQuests.FirstOrDefault(q => q.QuestId == questId);
            if (questState != null && !questState.IsCompleted && !questState.IsFailed)
            {
                _questEngine.EvaluateConditions(character, questId, questState, progressData);
                _questDatabase.SaveQuestState(character, questState);
            }
        }

        public void CompleteQuest(PlayerCharacter character, string questId)
        {
            var questState = character.ActiveQuests.FirstOrDefault(q => q.QuestId == questId);
            if (questState != null && questState.CurrentObjectives.All(o => o.IsCompleted))
            {
                questState.IsCompleted = true;
                character.CompletedQuests.Add(questState);
                character.ActiveQuests.Remove(questState);
                _questDatabase.SaveQuestState(character, questState);
                GiveRewards(character, questId);
            }
        }

        private bool CharacterHasQuest(PlayerCharacter character, string questId)
        {
            return character.ActiveQuests.Any(q => q.QuestId == questId);
        }

        private void GiveRewards(PlayerCharacter character, string questId)
        {
            var quest = _questDatabase.GetQuestDefinition(questId);
            foreach (var reward in quest.Rewards)
            {
                // Implement logic to give rewards (e.g., add items to inventory, give experience points, etc.)
            }
        }

        public List<PlayerQuestState> GetActiveQuests(PlayerCharacter character)
        {
            return character.ActiveQuests;
        }

        public List<Quest> GetAvailableQuests(PlayerCharacter character)
        {
            // Implement logic to determine available quests based on character state
            return new List<Quest>();
        }
    }
}
