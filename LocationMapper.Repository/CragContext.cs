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
            return base.SaveChanges();
        }
    }
}