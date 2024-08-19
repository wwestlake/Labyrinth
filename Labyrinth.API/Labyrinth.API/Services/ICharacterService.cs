using Labyrinth.API.Entities.Characters;
using FluentResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labyrinth.API.Services
{
    public interface ICharacterService
    {
        Task<List<Character>> GetCharactersAsync();
        Task<Character> CreateCharacterAsync(string name, CharacterClass characterClass, Race race);
        Task<Result> SubmitCharacterAsync(string characterId, IDictionary<string, int> submittedStats);
        Task UpdateCharacterAsync(string characterId, Character updatedCharacter);
        Task<Character> GetCharacterByIdAsync(string characterId); // Add this method
    }
}
