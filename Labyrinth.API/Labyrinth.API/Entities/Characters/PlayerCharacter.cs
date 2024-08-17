using FluentResults;

namespace Labyrinth.API.Entities.Characters
{
    public class PlayerCharacter : Character
    {
        public int ExperiencePoints { get; set; }  // XP points for leveling up
        public CharacterClass CharacterClass { get; set; }  // Character's class (e.g., Warrior, Mage)
        public string UserId { get; set; }  // ID of the user who owns this character

        public PlayerCharacter()
        {
            // Assign default behavior for actions
            Attack = DefaultAttack;
            Defend = DefaultDefend;
            UseAbility = DefaultUseAbility;
        }

        // Default attack behavior
        private Result DefaultAttack(Character target)
        {
            // Implement basic attack logic, e.g., reduce target health
            int damage = this.Strength; // Simplified example
            target.Health -= damage;

            return Result.Ok().WithSuccess($"Attacked {target.Name} for {damage} damage.");
        }

        // Default defend behavior
        private Result DefaultDefend()
        {
            // Implement basic defend logic, e.g., increase defense for a turn
            return Result.Ok().WithSuccess($"{Name} is defending.");
        }

        // Default use ability behavior
        private Result DefaultUseAbility(string abilityName, Character target)
        {
            // Implement ability logic, e.g., cast a spell on the target
            return Result.Ok().WithSuccess($"{Name} used {abilityName} on {target.Name}.");
        }
    }
}
