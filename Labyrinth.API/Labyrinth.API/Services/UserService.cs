using FirebaseAdmin.Auth;
using Google;
using Labyrinth.API.Common;

namespace Labyrinth.API.Services
{
    public class UserService : IUserService
    {
        private readonly LabyrinthDbContext _dbContext;

        public UserService(LabyrinthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AssignUserRole(string userId, Role role)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.Role = role;
                await _dbContext.SaveChangesAsync();

                // Also update the custom claims in Firebase
                await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userId, new Dictionary<string, object>
            {
                { "role", role.ToString() }
            });
            }
        }
    }
}
