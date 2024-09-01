namespace Labyrinth.API.Entities.Quests
{
    public class Quest
    {
        public string QuestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<QuestObjective> Objectives { get; set; } = new List<QuestObjective>();
        public List<QuestReward> Rewards { get; set; } = new List<QuestReward>();
        public List<string> Prerequisites { get; set; } = new List<string>();
        public List<string> FailureConditions { get; set; } = new List<string>();
    }

    public class QuestObjective
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class QuestReward
    {
        public string Type { get; set; } // E.g., "Experience", "Item", "Gold"
        public int Amount { get; set; }
        public string ItemId { get; set; } // If the reward is an item
    }
}
