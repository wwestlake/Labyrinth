using Labyrinth.API.Entities;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Driver;
using System.Security.Claims;

namespace Labyrinth.API.Services;

public class ClaimsTransformer : IClaimsTransformation
{
    private readonly IMongoCollection<ApplicationUser> _usersCollection;

    public ClaimsTransformer(IMongoClient mongoClient, string databaseName, string usersCollectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _usersCollection = database.GetCollection<ApplicationUser>(usersCollectionName);
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // Extract user ID from the claims
        var userIdClaim = principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return principal; // No user ID found, return principal as is
        }

        var userId = userIdClaim.Value;

        // Fetch the user from MongoDB based on the user ID
        var user = await _usersCollection.Find(u => u.UserId == userId).FirstOrDefaultAsync();

        if (user == null)
        {
            return principal; // User not found, return principal as is
        }

        // Create a new identity with existing claims
        var identity = (ClaimsIdentity)principal.Identity;

        // Add the role from MongoDB to the user's claims
        if (!identity.HasClaim(ClaimTypes.Role, user.Role.ToString()))
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
        }

        return principal; // Return the modified principal with new roles
    }
}
