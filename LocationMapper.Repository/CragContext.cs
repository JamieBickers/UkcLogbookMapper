using System;
using System.Linq;
using LocationMapper.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocationMapper.Repository
{
    public class CragContext : DbContext
    {
        public CragContext(DbContextOptions<CragContext> options) : base(options)
        {
        }

        public DbSet<Crag> Crag { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Crag>().HasKey(crag => crag.ID);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            UpdateUpdatedProperty<Crag>();

            return base.SaveChanges();
        }

        private void UpdateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(entity => entity.State == EntityState.Added || entity.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("UpdatedTimestamp").CurrentValue = DateTime.UtcNow;
            }
        }
    }
}