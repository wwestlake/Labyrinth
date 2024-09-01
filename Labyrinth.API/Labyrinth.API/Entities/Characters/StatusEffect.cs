using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Labyrinth.API.Entities.Characters
{
    public class StatusEffect
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Unique identifier for the status effect

        [BsonElement("type")]
        public StatusEffectType Type { get; set; }  // Type of the status effect (e.g., Buff, Debuff, Poison)

        [BsonElement("description")]
        public string Description { get; set; }  // Description of the status effect

        [BsonElement("duration")]
        public TimeSpan Duration { get; set; }  // Duration of the effect

        [BsonElement("startTime")]
        public DateTime StartTime { get; set; }  // Time when the effect was applied

        [BsonElement("isExpired")]
        public bool IsExpired => DateTime.UtcNow > StartTime + Duration;  // Determines if the effect is expired

        [BsonElement("modifier")]
        public int Modifier { get; set; }  // A numeric modifier (e.g., +10% to Strength, -20 Health)

        // Example of an effect application, can be customized further
        public void ApplyEffect(Character character)
        {
            if (!IsExpired)
            {
                // Modify character stats or apply effects based on the effect type
                switch (Type)
                {
                    case StatusEffectType.Buff:
                        // Apply positive modifier
                        break;
                    case StatusEffectType.Debuff:
                        // Apply negative modifier
                        break;
                        // More cases based on different StatusEffectTypes
                }
            }
        }

        // Method to remove or reverse effect
        public void RemoveEffect(Character character)
        {
            if (IsExpired)
            {
                // Revert any changes made by the effect
            }
        }
    }

    public enum StatusEffectType
    {
        Buff,
        Debuff,
        Poison,
        Stun,
        Freeze,
        Burn,
        HealOverTime,
        DamageOverTime,
        // Add more types as needed
    }
}
