using Microsoft.EntityFrameworkCore;
using NZWalks.API.Modules.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> options) : base(options)
        {

        }

        // Important: with this DbSet, you're telling to EF to create that table if it doesn't exist and to "talk" with those persistent data
        public DbSet<Region> Region { get; set; }
        public DbSet<Walk> Walk { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
    }
}
