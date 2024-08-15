using Labyrinth.API.Entities;
using Microsoft.EntityFrameworkCore;



// Entity Framework DbContext
public partial class LabyrinthDbContext : DbContext
{
    public LabyrinthDbContext(DbContextOptions<LabyrinthDbContext> options)
    : base(options) { }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Character> Characters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>()
            .Property(e => e.Role)
            .HasConversion<string>(); // Stores the enum as a string in the database

        base.OnModelCreating(modelBuilder);
    }

}
