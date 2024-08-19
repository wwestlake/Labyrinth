using Labyrinth.API.Entities.Characters;

namespace Labyrinth.API.Dto
{
    // Request model for generating a new character
    public class CharacterCreationRequest
    {
        public string Name { get; set; }
        public CharacterClass CharacterClass { get; set; }
        public Race Race { get; set; }
    }
}
