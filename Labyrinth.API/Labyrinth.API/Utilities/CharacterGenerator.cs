using Labyrinth.API.Entities.Characters;

namespace Labyrinth.API.Utilities
{
    public static class CharacterGenerator
    {
        // Method to generate a character with base stats
        public static Character GenerateCharacter(string name, CharacterClass characterClass, Race race)
        {
            var character = new PlayerCharacter
            {
                Name = name,
                CharacterClass = characterClass,
                Race = race,
                Level = 1,
                ExperiencePoints = 0,
                Health = GenerateStat(characterClass, race, "Health"),
                Mana = GenerateStat(characterClass, race, "Mana"),
                Strength = GenerateStat(characterClass, race, "Strength"),
                Dexterity = GenerateStat(characterClass, race, "Dexterity"),
                Constitution = GenerateStat(characterClass, race, "Constitution"),
                Intelligence = GenerateStat(characterClass, race, "Intelligence"),
                Wisdom = GenerateStat(characterClass, race, "Wisdom"),
                Charisma = GenerateStat(characterClass, race, "Charisma"),
                Luck = GenerateStat(characterClass, race, "Luck")
            };

            return character;
        }

        // Private method to generate a stat based on class and race (Placeholder implementation)
        private static Stat GenerateStat(CharacterClass characterClass, Race race, string statName)
        {
            // Implement your logic for generating stats here, using class and race
            return new Stat
            {
                BaseValue = 10, // Placeholder value
            };
        }
    }
}
