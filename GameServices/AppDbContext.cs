using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace GameServices
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Charactere> Characters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Ennemie> Ennemies { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GameSave> GameSaves { get; set; }

        
    }
}
