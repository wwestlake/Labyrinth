using Labyrinth.API.Common;
using Labyrinth.API.Entities;

namespace Labyrinth.API.Services
{
    public interface IUserService
    {
        Task AssignUserRole(string userId, Role role);
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(string userId);
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByUidAsync(string uid);
        Task UpdateUserAsync(string userId, ApplicationUser updatedUser);
    }
}
