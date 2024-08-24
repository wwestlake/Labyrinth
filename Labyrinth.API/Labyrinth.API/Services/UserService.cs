using FirebaseAdmin.Auth;
using Labyrinth.API.Common;
using Labyrinth.API.Entities;
using MongoDB.Driver;

namespace Labyrinth.API.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<ApplicationUser> _usersCollection;

    public UserService(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("Labyrinth");
        _usersCollection = database.GetCollection<ApplicationUser>("ApplicationUsers");
    }

    // Get current user based on Firebase UID
    public async Task<ApplicationUser> GetUserByUidAsync(string uid)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(u => u.UserId, uid);
        try
        {
            var result = await _usersCollection.Find(filter).FirstOrDefaultAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    // Get all users
    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await _usersCollection.Find(Builders<ApplicationUser>.Filter.Empty).ToListAsync();
    }

    // Create a new user
    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
    {
        await _usersCollection.InsertOneAsync(user);
        return user;
    }

    // Update an existing user
    public async Task UpdateUserAsync(string userId, ApplicationUser updatedUser)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(u => u.UserId, userId);
        var result = await _usersCollection.ReplaceOneAsync(filter, updatedUser);

        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }
    }

    // Delete a user by their UserId
    public async Task DeleteUserAsync(string userId)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(u => u.UserId, userId);
        var result = await _usersCollection.DeleteOneAsync(filter);

        if (result.DeletedCount == 0)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }
    }

    // Assign a role to a user (already implemented)
    public async Task AssignUserRole(string userId, Role role)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(u => u.UserId, userId);
        var update = Builders<ApplicationUser>.Update.Set(u => u.Role, role);

        var result = await _usersCollection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount > 0)
        {
            // Also update the custom claims in Firebase
            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userId, new Dictionary<string, object>
            {
                { "role", role.ToString() }
            });
        }
        else
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }
    }
}
