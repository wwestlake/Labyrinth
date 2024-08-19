using MongoDB.Driver;
using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Utilities;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labyrinth.API.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IMongoCollection<Character> _charactersCollection;

        public CharacterService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Labyrinth");
            _charactersCollection = database.GetCollection<Character>("Characters");
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            return await _charactersCollection.Find(Builders<Character>.Filter.Empty).ToListAsync();
        }

        public async Task<Character> CreateCharacterAsync(string name, CharacterClass characterClass, Race race)
        {
            var character = CharacterGenerator.GenerateCharacter(name, characterClass, race);
            await _charactersCollection.InsertOneAsync(character);
            return character;
        }

        public async Task<Result> SubmitCharacterAsync(string characterId, IDictionary<string, int> submittedStats)
        {
            var filter = Builders<Character>.Filter.Eq(c => c.Id, characterId);
            var character = await _charactersCollection.Find(filter).FirstOrDefaultAsync();

            if (character == null)
            {
                return Result.Fail("Character not found.");
            }

            if (CharacterValidator.ValidateCharacterStats(character, submittedStats))
            {
                UpdateCharacterStats(character, submittedStats);
                await UpdateCharacterAsync(characterId, character);
                return Result.Ok();
            }
            else
            {
                NotifyAdminsOfFraudAttempt(character, submittedStats);
                return Result.Fail("Invalid character stats. Possible fraud attempt detected.");
            }
        }

        public async Task UpdateCharacterAsync(string characterId, Character updatedCharacter)
        {
            var filter = Builders<Character>.Filter.Eq(c => c.Id, characterId);
            var result = await _charactersCollection.ReplaceOneAsync(filter, updatedCharacter);

            if (result.MatchedCount == 0)
            {
                throw new KeyNotFoundException($"Character with ID {characterId} not found.");
            }
        }

        public async Task<Character> GetCharacterByIdAsync(string characterId)
        {
            var filter = Builders<Character>.Filter.Eq(c => c.Id, characterId);
            return await _charactersCollection.Find(filter).FirstOrDefaultAsync();
        }

        private void UpdateCharacterStats(Character character, IDictionary<string, int> submittedStats)
        {
            character.Health.BaseValue = submittedStats["Health"];
            character.Mana.BaseValue = submittedStats["Mana"];
            character.Strength.BaseValue = submittedStats["Strength"];
            character.Dexterity.BaseValue = submittedStats["Dexterity"];
            character.Constitution.BaseValue = submittedStats["Constitution"];
            character.Intelligence.BaseValue = submittedStats["Intelligence"];
            character.Wisdom.BaseValue = submittedStats["Wisdom"];
            character.Charisma.BaseValue = submittedStats["Charisma"];
            character.Luck.BaseValue = submittedStats["Luck"];
        }

        private void NotifyAdminsOfFraudAttempt(Character character, IDictionary<string, int> submittedStats)
        {
            // Implement logic to notify admins, e.g., sending an email or logging the event
        }
    }
}
