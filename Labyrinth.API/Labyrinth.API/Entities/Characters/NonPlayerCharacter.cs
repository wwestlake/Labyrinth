using FluentResults;

namespace Labyrinth.API.Entities.Characters
{
    public class NonPlayerCharacter : Character
    {
        public int AggressionLevel { get; set; }  // How aggressive this NPC is

        public NonPlayerCharacter()
        {
            // Assign default behavior for actions or customize them
            Attack = DefaultAttack;
            Defend = DefaultDefend;
            UseAbility = DefaultUseAbility;
        }

        // Default attack behavior for NPCs
        private Result DefaultAttack(Character target)
        {
            // Implement NPC-specific attack logic
            int damage = this.Strength; // Simplified example
            target.Health -= damage;

            return Result.Ok().WithSuccess($"NPC {Name} attacked {target.Name} for {damage} damage.");
        }

        // Default defend behavior for NPCs
        private Result DefaultDefend()
        {
            // Implement NPC-specific defend logic
            return Result.Ok().WithSuccess($"NPC {Name} is defending.");
        }

        // Default use ability behavior for NPCs
        private Result DefaultUseAbility(string abilityName, Character target)
        {
            // Implement NPC-specific ability logic
            return Result.Ok().WithSuccess($"NPC {Name} used {abilityName} on {target.Name}.");
        }
    }
}
