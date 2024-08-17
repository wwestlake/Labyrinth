using FluentResults;
using System;

namespace Labyrinth.API.Entities.Characters
{
    public abstract class Character
    {
        public string Id { get; set; }  // Unique identifier for the character
        public string Name { get; set; }  // Character's name
        public int Level { get; set; }  // Character's level
        public int Health { get; set; }  // Character's health points
        public int Mana { get; set; }  // Character's mana points
        public int Strength { get; set; }  // Character's strength stat
        public int Dexterity { get; set; }  // Character's dexterity stat
        public int Constitution { get; set; }  // Character's constitution stat
        public int Intelligence { get; set; }  // Character's intelligence stat
        public int Wisdom { get; set; }  // Character's wisdom stat
        public int Charisma { get; set; }  // Character's charisma stat
        public int Luck { get; set; }  // Character's luck stat

        // Lambda functions for character actions
        public Func<Character, Result> Attack { get; set; }
        public Func<Result> Defend { get; set; }
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
    }
}
