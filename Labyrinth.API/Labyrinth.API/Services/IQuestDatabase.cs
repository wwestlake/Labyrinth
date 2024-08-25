using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Entities.Quests;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth.API.Services
{
    public interface IQuestDatabase
    {
        void SaveQuestState(PlayerCharacter character, PlayerQuestState questState);
        PlayerQuestState LoadQuestState(PlayerCharacter character, string questId);
        Quest GetQuestDefinition(string questId);
        List<PlayerQuestState> GetActiveQuests(PlayerCharacter character);
    }

    public class QuestDatabase : IQuestDatabase
    {
        private readonly IMongoCollection<PlayerQuestState> _questStatesCollection;
        private readonly IMongoCollection<Quest> _questsCollection;

        public QuestDatabase(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Labyrinth");
            _questStatesCollection = database.GetCollection<PlayerQuestState>("QuestStates");
            _questsCollection = database.GetCollection<Quest>("Quests");
        }

        public void SaveQuestState(PlayerCharacter character, PlayerQuestState questState)
        {
            var filter = Builders<PlayerQuestState>.Filter.And(
                Builders<PlayerQuestState>.Filter.Eq("QuestId", questState.QuestId),
                Builders<PlayerQuestState>.Filter.Eq("CharacterId", character.Id)
            );

            _questStatesCollection.ReplaceOne(filter, questState, new ReplaceOptions { IsUpsert = true });
        }

        public PlayerQuestState LoadQuestState(PlayerCharacter character, string questId)
        {
            var filter = Builders<PlayerQuestState>.Filter.And(
                Builders<PlayerQuestState>.Filter.Eq("QuestId", questId),
                Builders<PlayerQuestState>.Filter.Eq("CharacterId", character.Id)
            );

            return _questStatesCollection.Find(filter).FirstOrDefault();
        }

        public Quest GetQuestDefinition(string questId)
        {
            var filter = Builders<Quest>.Filter.Eq("QuestId", questId);
            return _questsCollection.Find(filter).FirstOrDefault();
        }

        public List<PlayerQuestState> GetActiveQuests(PlayerCharacter character)
        {
            var filter = Builders<PlayerQuestState>.Filter.And(
                Builders<PlayerQuestState>.Filter.Eq("CharacterId", character.Id),
                Builders<PlayerQuestState>.Filter.Eq("IsCompleted", false),
                Builders<PlayerQuestState>.Filter.Eq("IsFailed", false)
            );

            return _questStatesCollection.Find(filter).ToList();
        }
    }
}
