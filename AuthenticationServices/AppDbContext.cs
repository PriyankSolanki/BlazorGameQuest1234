using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace AuthenticationServices;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Charactere> Characters { get; set; }
    public DbSet<Ennemie> Ennemies { get; set; }
    public DbSet<Player> Players { get; set; } 
    public DbSet<Room> Rooms { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ennemie>().ToTable("Ennemies");
        modelBuilder.Entity<Player>().ToTable("Players");
    }

}