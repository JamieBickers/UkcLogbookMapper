using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationsMapper.Database
{
    public class CragLocationContext : DbContext
    {
        private DbContextOptionsBuilder options;

        public CragLocationContext(DbContextOptions<CragLocationContext> options)
            : base(options)
        { }

        public DbSet<CragLocation> CragLocations { get; set; }
    }
}
