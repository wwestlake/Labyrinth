using Labyrinth.API.Entities;
using Labyrinth.API.Entities.Characters;
using System;

namespace Labyrinth.API.Services
{
    // Interface for PlayerService
    public interface IPlayerService
    {
        Task<Player> CreatePlayerAsync(ApplicationUser user, PlayerCharacter character);
        Task<Player> GetPlayerAsync(string userId, string characterId);
        Task UpdatePlayerAsync(Player player);
        Task SetPlayerOnlineAsync(string userId, string characterId);
        Task SetPlayerOfflineAsync(string userId, string characterId);
        Task DeletePlayerAsync(string playerId);
    }
}
