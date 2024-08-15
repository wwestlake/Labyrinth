using Google;
using Labyrinth.API.Common;
using Labyrinth.API.Entities;
using Labyrinth.API.Services;
using Microsoft.EntityFrameworkCore;

namespace Labyrinth.API
{
    public class DevDataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LabyrinthDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<LabyrinthDbContext>>()))
            {
                // Look for any users already in the database.
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                // Add users with roles
                var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserId = "Q9qmJLFz81TdhDpWIRKLzVDMRoZ2",
                    Email = "wweatlake1234@gmail.com",
                    DisplayName = "Bill",
                    Role = Role.Owner
                },
                new ApplicationUser
                {
                    UserId = "EI9CDrZtzUMykKCACfgHivJJ3Cx2",
                    Email = "wwestlake@lagdaemon.com",
                    DisplayName = "Admin One",
                    Role = Role.Administrator
                },
                new ApplicationUser
                {
                    UserId = "oKF5suoqnzcFBibZXF6QZr1ov1y1",
                    Email = "test@lagdaemon.com",
                    DisplayName = "User 1",
                    Role = Role.User
                }
            };

                context.Users.AddRange(users);
                context.SaveChanges();

                if (!context.Characters.Any())
                {
                    context.Characters.AddRange(
                        new Character { Name = "Test Entity 1" },
                        new Character { Name = "Test Entity 2" }
                    );
                    context.SaveChanges();
                }

                // Assign roles in Firebase
                var userService = serviceProvider.GetRequiredService<IUserService>();
                foreach (var user in users)
                {
                    userService.AssignUserRole(user.UserId, user.Role).Wait();
                }
            }
        }

    }
}
