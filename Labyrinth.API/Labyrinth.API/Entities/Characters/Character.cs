using FluentResults;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Labyrinth.API.Entities.Characters
{
    public class Character
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Unique identifier for the character

        [BsonRepresentation(BsonType.ObjectId)]
        public string Owner { get; set; }
        public CharacterClass CharacterClass { get; set; }  // Character's class (e.g., Warrior, Mage)

        public string Name { get; set; }  // Character's name
        public string Description { get; set; } // description or BIO of the character
        public Race Race { get; set; }  // Character's race
        public int Level { get; set; }  // Character's level
        public int ExperiencePoints { get; set; }  // Character's experience points

        public Stat Health { get; set; }  // Character's health points
        public Stat Mana { get; set; }  // Character's mana points
        public Stat Strength { get; set; }  // Character's strength stat
        public Stat Dexterity { get; set; }  // Character's dexterity stat
        public Stat Constitution { get; set; }  // Character's constitution stat
        public Stat Intelligence { get; set; }  // Character's intelligence stat
        public Stat Wisdom { get; set; }  // Character's wisdom stat
        public Stat Charisma { get; set; }  // Character's charisma stat
        public Stat Luck { get; set; }  // Character's luck stat

        [BsonRepresentation(BsonType.ObjectId)]
        public string CurrentRoomId { get; set; }  // ID of the room the character is currently in

        // Lambda functions for character actions
        [BsonIgnore, JsonIgnore] // Exclude from serialization
        public Func<Character, Result> Attack { get; set; }

        [BsonIgnore, JsonIgnore] // Exclude from serialization
        public Func<Result> Defend { get; set; }

        [BsonIgnore, JsonIgnore] // Exclude from serialization
        public Func<string, Character, Result> UseAbility { get; set; }

        // Default actions using the Func<> properties
        public Result ExecuteAttack(Character target)
        {
            return Attack != null
                ? Attack(target)
                : Result.Fail("Attack action not defined.");
        }

        public Result ExecuteDefend()
        {
            return Defend != null
                ? Defend()
                : Result.Fail("Defend action not defined.");
        }

        public Result ExecuteUseAbility(string abilityName, Character target)
        {
            return UseAbility != null
                ? UseAbility(abilityName, target)
                : Result.Fail("Use Ability action not defined.");
        }

        // Method to update and recalculate all stats
        public void UpdateStats()
        {
            Health.RecalculateTotalValue();
            Mana.RecalculateTotalValue();
            Strength.RecalculateTotalValue();
            Dexterity.RecalculateTotalValue();
            Constitution.RecalculateTotalValue();
            Intelligence.RecalculateTotalValue();
            Wisdom.RecalculateTotalValue();
            Charisma.RecalculateTotalValue();
            Luck.RecalculateTotalValue();
        }

        // Method to add experience points and level up if necessary
        public void AddExperience(int experience)
        {
            ExperiencePoints += experience;
            CheckLevelUp();
        }

        // Method to check if the character has enough XP to level up
        private void CheckLevelUp()
        {
            int xpNeededForNextLevel = CalculateXpNeededForNextLevel();
            while (ExperiencePoints >= xpNeededForNextLevel)
            {
                LevelUp();
                xpNeededForNextLevel = CalculateXpNeededForNextLevel();
            }
        }

        // Method to level up the character
        private void LevelUp()
        {
            Level++;
            // Additional logic for leveling up (e.g., increasing stats) can be added here
        }

        // Method to calculate the XP needed for the next level
        private int CalculateXpNeededForNextLevel()
        {
            // Example calculation: XP needed for the next level increases exponentially
            return Level * 1000;
        }
    }
}
