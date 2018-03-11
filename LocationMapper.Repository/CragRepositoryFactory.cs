using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationMapper.Repository
{
    public static class CragRepositoryFactory
    {
        public static ICragRepository GetCragRepository(string connectionString)
        {
            var dbOptionsBuilder = new DbContextOptionsBuilder<CragContext>();
            dbOptionsBuilder.UseNpgsql(connectionString).EnableSensitiveDataLogging();

            var cragContext = new CragContext(dbOptionsBuilder.Options);
           return new CragRepository(cragContext);
        }
    }
}
