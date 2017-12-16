using Microsoft.EntityFrameworkCore;

namespace LocationsMapper.Database
{
    public class CragLocationContext : DbContext
    {
        private DbContextOptionsBuilder options;

        public CragLocationContext(DbContextOptions<CragLocationContext> options)
            : base(options)
        { }

        public DbSet<CragLocation> CragLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CragLocation>()
                .Property(location => location.Latitude)
                .HasColumnType("decimal(38, 8)");

            modelBuilder.Entity<CragLocation>()
                .Property(location => location.Longitude)
                .HasColumnType("decimal(38, 8)");
        }
    }
}
