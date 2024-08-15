using Labyrinth.API.Common;

namespace Labyrinth.API.Entities
{
    public class ApplicationUser
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public Role Role { get; set; } // Enum property
    }
}

