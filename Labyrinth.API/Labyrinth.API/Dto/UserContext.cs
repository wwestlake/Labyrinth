using Labyrinth.API.Entities;
using Labyrinth.API.Entities.Characters;

namespace Labyrinth.API.Dto
{
    public class UserContext
    {
        public ApplicationUser User { get; set; }
        public Character CurrentCharacter { get; set; }
        public List<Character> AvailableCharacters { get; set; }
        public Room CurrentRoom { get; set; }

        public UserContext(ApplicationUser user, Character currentCharacter, List<Character> availableCharacters, Room currentRoom)
        {
            User = user;
            CurrentCharacter = currentCharacter;
            AvailableCharacters = availableCharacters;
            CurrentRoom = currentRoom;
        }

    }
}
