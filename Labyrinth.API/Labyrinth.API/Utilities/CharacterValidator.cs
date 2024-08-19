using Labyrinth.API.Entities.Characters;
using System.Collections.Generic;

namespace Labyrinth.API.Utilities
{
    public static class CharacterValidator
    {
        // Method to validate the character stats
        public static bool ValidateCharacterStats(Character character, IDictionary<string, int> submittedStats)
        {
            int originalStatSum = CalculateStatSum(character);
            int submittedStatSum = CalculateStatSum(submittedStats);

            return originalStatSum == submittedStatSum;
        }

        // Private method to calculate the sum of character stats
        private static int CalculateStatSum(Character character)
        {
            return character.Health.BaseValue
                + character.Mana.BaseValue
                + character.Strength.BaseValue
                + character.Dexterity.BaseValue
                + character.Constitution.BaseValue
                + character.Intelligence.BaseValue
                + character.Wisdom.BaseValue
                + character.Charisma.BaseValue
                + character.Luck.BaseValue;
        }

        // Overloaded method to calculate the sum of submitted stats
        private static int CalculateStatSum(IDictionary<string, int> stats)
        {
            int sum = 0;
            foreach (var stat in stats)
            {
                sum += stat.Value;
            }
            return sum;
        }
    }
}
