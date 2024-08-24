using Labyrinth.API.Entities;
using Labyrinth.API.Entities.Characters;
using Labyrinth.API.Entities.Items;
using Labyrinth.API.Entities.Rules;
using MongoDB.Bson;

namespace Labyrinth.API.InMemoryDatabase
{
    public static class InMemoryDatabase
    {
        public static List<Room> Rooms { get; private set; }
        public static List<PlayerCharacter> PlayerCharacters { get; private set; }
        public static List<NonPlayerCharacter> NonPlayerCharacters { get; private set; }
        public static List<Item> Items { get; private set; }
        public static List<Rule> Rules { get; private set; }

        static InMemoryDatabase()
        {
            PopulateRooms();
            PopulateCharacters();
            PopulateItems();
            PopulateRules();
        }

        private static void PopulateRooms()
        {
            Rooms = new List<Room>();

            // Create 7 rooms: a lobby and one in each cardinal direction (N, S, E, W, NE, NW)
            var lobby = new Room { Id = ObjectId.GenerateNewId(), Name = "Lobby", Description = "The central room" };
            var northRoom = new Room { Id = ObjectId.GenerateNewId(), Name = "North Room", Description = "A room to the north" };
            var southRoom = new Room { Id = ObjectId.GenerateNewId(), Name = "South Room", Description = "A room to the south" };
            var eastRoom = new Room { Id = ObjectId.GenerateNewId(), Name = "East Room", Description = "A room to the east" };
            var westRoom = new Room { Id = ObjectId.GenerateNewId(), Name = "West Room", Description = "A room to the west" };
            var northEastRoom = new Room { Id = ObjectId.GenerateNewId(), Name = "NorthEast Room", Description = "A room to the northeast" };
            var northWestRoom = new Room { Id = ObjectId.GenerateNewId(), Name = "NorthWest Room", Description = "A room to the northwest" };

            // Create exits - adjust if the "LeadsTo" property does not exist
            lobby.Exits.Add(new Exit { Direction = "North" });
            lobby.Exits.Add(new Exit { Direction = "South" });
            lobby.Exits.Add(new Exit { Direction = "East" });
            lobby.Exits.Add(new Exit { Direction = "West" });
            lobby.Exits.Add(new Exit { Direction = "NorthEast" });
            lobby.Exits.Add(new Exit { Direction = "NorthWest" });

            // Add rooms to list
            Rooms.Add(lobby);
            Rooms.Add(northRoom);
            Rooms.Add(southRoom);
            Rooms.Add(eastRoom);
            Rooms.Add(westRoom);
            Rooms.Add(northEastRoom);
            Rooms.Add(northWestRoom);
        }

        private static void PopulateCharacters()
        {
            PlayerCharacters = new List<PlayerCharacter>();
            NonPlayerCharacters = new List<NonPlayerCharacter>();

            // Create a player character
            var playerCharacter = new PlayerCharacter
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Hero",
                CharacterClass = CharacterClass.Warrior, // Adjust property names to actual implementation
                Race = Race.Human,
                Health = new Stat { BaseValue = 100 },
                Mana = new Stat { BaseValue = 50 },
                Strength = new Stat { BaseValue = 20 },
            };

            // Create a non-player character
            var npcCharacter = new NonPlayerCharacter
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Guardian",
                CharacterClass = CharacterClass.Warrior, // Adjust property names to actual implementation
                Race = Race.Orc,
                Health = new Stat { BaseValue = 150 },
                Mana = new Stat { BaseValue = 30 },
                Strength = new Stat { BaseValue = 25 },
            };

            PlayerCharacters.Add(playerCharacter);
            NonPlayerCharacters.Add(npcCharacter);
        }

        private static void PopulateItems()
        {
            Items = new List<Item>();

            // Create some weapons
            var sword = new Weapon("Sword", "A sharp blade", 15, "Melee", 100, 0.1, "Fast");
            var bow = new Weapon("Bow", "A ranged weapon", 10, "Ranged", 50, 0.2, "Medium");

            // Create some armor
            var helmet = new Armor("Helmet", "A sturdy helmet", 10, "Helmet", 50, 5, "None");

            // Create consumables
            var healthPotion = new Consumable("Health Potion", "Restores health", "Healing", 10, 1);
            var manaPotion = new Consumable("Mana Potion", "Restores mana", "Mana", 10, 1);

            // Create treasures
            var gold = new Treasure("Gold", "Shiny gold coins", 100, "Common", 1);
            var rareGem = new Treasure("Rare Gem", "A precious stone", 1000, "Rare", 1);

            // Add items to list
            Items.Add(sword);
            Items.Add(bow);
            Items.Add(helmet);
            Items.Add(healthPotion);
            Items.Add(manaPotion);
            Items.Add(gold);
            Items.Add(rareGem);

            // Place items in containers or on characters
            var chest = new Container("Chest", "A sturdy chest", 10, 100);
            chest.Store(gold);
            chest.Store(rareGem);
            Rooms[0].Items.Add(chest); // Place chest in the lobby
        }

        private static void PopulateRules()
        {
            Rules = new List<Rule>();

            // Create a simple rule
            var healthCheckCondition = new Condition
            {
                Type = "StatCheck",
                Field = "Health",
                Value = 50
            };

            var healAction = new Labyrinth.API.Entities.Rules.Action
            {
                Type = "Heal",
                Value = 20
            };

            var healingRule = new Rule
            {
                Id = 1,
                Name = "Healing Rule",
                Description = "Automatically heal if health falls below 50",
                Conditions = new List<Condition> { healthCheckCondition },
                Actions = new List<Labyrinth.API.Entities.Rules.Action> { healAction }
            };

            Rules.Add(healingRule);
        }

        public static void PrintDatabaseContents()
        {
            Console.WriteLine("Rooms:");
            foreach (var room in Rooms)
            {
                Console.WriteLine($"- {room.Name}: {room.Description}");
                foreach (var exit in room.Exits)
                {
                    Console.WriteLine($"  -> Exit to: {exit.Direction}");
                }
            }

            Console.WriteLine("\nCharacters:");
            foreach (var character in PlayerCharacters)
            {
                Console.WriteLine($"- {character.Name} ({character.CharacterClass}): Health = {character.Health.BaseValue}");
            }

            foreach (var npc in NonPlayerCharacters)
            {
                Console.WriteLine($"- {npc.Name} ({npc.CharacterClass}): Health = {npc.Health.BaseValue}");
            }

            Console.WriteLine("\nItems:");
            foreach (var item in Items)
            {
                Console.WriteLine($"- {item.Name} ({item.Type}): {item.Description}");
            }

            Console.WriteLine("\nRules:");
            foreach (var rule in Rules)
            {
                Console.WriteLine($"- {rule.Name}: {rule.Description}");
            }
        }
    }
}
