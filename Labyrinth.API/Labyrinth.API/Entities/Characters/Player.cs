using Labyrinth.API.Entities.Items;
using Labyrinth.API.Entities.Quests;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Characters
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Unique identifier for the player entity

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }  // The ID of the user playing this character

        [BsonRepresentation(BsonType.ObjectId)]
        public string CharacterId { get; set; }  // The ID of the character template

        public string CharacterName { get; set; }  // The name of the character, used for display purposes

        public string CurrentRoomId { get; set; }  // ID of the room the player is currently in

        // Dynamic stats that change during gameplay
        public Stat Health { get; set; }
        public Stat Mana { get; set; }
        public Stat Strength { get; set; }
        public Stat Dexterity { get; set; }
        public Stat Constitution { get; set; }
        public Stat Intelligence { get; set; }
        public Stat Wisdom { get; set; }
        public Stat Charisma { get; set; }
        public Stat Luck { get; set; }

        // Inventory and Equipment
        public List<Item> Inventory { get; set; } = new List<Item>();  // Items currently held by the player
        public List<Item> Equipment { get; set; } = new List<Item>();  // Items currently equipped by the player

        // Status effects, buffs, debuffs, etc.
        public List<StatusEffect> ActiveStatusEffects { get; set; } = new List<StatusEffect>();

        // Quest progression specific to this player-character
        public List<PlayerQuestState> ActiveQuests { get; set; } = new List<PlayerQuestState>();
        public List<PlayerQuestState> CompletedQuests { get; set; } = new List<PlayerQuestState>();
        public List<PlayerQuestState> FailedQuests { get; set; } = new List<PlayerQuestState>();

        // Is the player currently online?
        public bool IsOnline { get; set; }

        public Player() { }

        public Player(ApplicationUser user, PlayerCharacter character)
        {
            UserId = user.Id;
            CharacterId = character.Id;
            CharacterName = character.Name;
            CurrentRoomId = character.CurrentRoomId;

            // Initialize stats from the character template
            Health = character.Health;
            Mana = character.Mana;
            Strength = character.Strength;
            Dexterity = character.Dexterity;
            Constitution = character.Constitution;
            Intelligence = character.Intelligence;
            Wisdom = character.Wisdom;
            Charisma = character.Charisma;
            Luck = character.Luck;

            // Initialize quests and other states
            ActiveQuests = character.ActiveQuests;
            CompletedQuests = character.CompletedQuests;
            FailedQuests = character.FailedQuests;
        }

        // Method to update dynamic state when necessary
        public void UpdateState(PlayerCharacter character)
        {
            Health = character.Health;
            Mana = character.Mana;
            Strength = character.Strength;
            Dexterity = character.Dexterity;
            Constitution = character.Constitution;
            Intelligence = character.Intelligence;
            Wisdom = character.Wisdom;
            Charisma = character.Charisma;
            Luck = character.Luck;
        }
    }
}
