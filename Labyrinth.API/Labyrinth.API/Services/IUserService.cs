using Labyrinth.API.Common;

namespace Labyrinth.API.Services
{
    public interface IUserService
    {
        Task AssignUserRole(string userId, Role role);
    }
}
