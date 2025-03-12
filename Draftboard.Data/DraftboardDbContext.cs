using Draftboard.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Draftboard.Data
{
    public class DraftboardDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStats> PlayerStats { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<PlayerPosition> PlayerPositions { get; set; }

        public DraftboardDbContext(DbContextOptions options) : base(options) { }
    }
}
