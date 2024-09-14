using Labyrinth.API.Entities;
using Labyrinth.API.Entities.Characters;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labyrinth.API.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IMongoCollection<Player> _playersCollection;

        public PlayerService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Labyrinth");
            _playersCollection = database.GetCollection<Player>("Players");
        }

        // Create a new Player record
        public async Task<Player> CreatePlayerAsync(ApplicationUser user, PlayerCharacter character)
        {
            var player = new Player(user, character);
            await _playersCollection.InsertOneAsync(player);
            return player;
        }

        // Get a Player record by UserId and CharacterId
        public async Task<Player> GetPlayerAsync(string userId, string characterId)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.UserId, userId) &
                         Builders<Player>.Filter.Eq(p => p.CharacterId, characterId);

            return await _playersCollection.Find(filter).FirstOrDefaultAsync();
        }

        // Update an existing Player's state
        public async Task UpdatePlayerAsync(Player player)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
            var update = Builders<Player>.Update
                .Set(p => p.CurrentRoomId, player.CurrentRoomId)
                .Set(p => p.Health, player.Health)
                .Set(p => p.Mana, player.Mana)
                .Set(p => p.Strength, player.Strength)
                .Set(p => p.Dexterity, player.Dexterity)
                .Set(p => p.Constitution, player.Constitution)
                .Set(p => p.Intelligence, player.Intelligence)
                .Set(p => p.Wisdom, player.Wisdom)
                .Set(p => p.Charisma, player.Charisma)
                .Set(p => p.Luck, player.Luck)
                .Set(p => p.Inventory, player.Inventory)
                .Set(p => p.Equipment, player.Equipment)
                .Set(p => p.ActiveStatusEffects, player.ActiveStatusEffects)
                .Set(p => p.ActiveQuests, player.ActiveQuests)
                .Set(p => p.CompletedQuests, player.CompletedQuests)
                .Set(p => p.FailedQuests, player.FailedQuests)
                .Set(p => p.IsOnline, player.IsOnline);

            await _playersCollection.UpdateOneAsync(filter, update);
        }

        // Mark a Player as online
        public async Task SetPlayerOnlineAsync(string userId, string characterId)
        {
            var player = await GetPlayerAsync(userId, characterId);
            if (player != null)
            {
                player.IsOnline = true;
                await UpdatePlayerAsync(player);
            }
        }

        // Mark a Player as offline
        public async Task SetPlayerOfflineAsync(string userId, string characterId)
        {
            var player = await GetPlayerAsync(userId, characterId);
            if (player != null)
            {
                player.IsOnline = false;
                await UpdatePlayerAsync(player);
            }
        }

        // Delete a Player record (e.g., if a character is deleted or no longer used)
        public async Task DeletePlayerAsync(string playerId)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
            await _playersCollection.DeleteOneAsync(filter);
        }
    }
}
