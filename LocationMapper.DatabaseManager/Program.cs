using LocationMapper.Repository;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace LocationMapper.DatabaseManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var connString = "Host=localhost;Username=UkcLogbookMapper;Password=qwerty;Database=Ukc";
            var dbOptions = new DbContextOptionsBuilder<CragContext>();
            dbOptions.UseNpgsql(connString);
            var cragContext = new CragContext(dbOptions.Options);
            var cragRepo = new CragRepository(cragContext);

            var crag = cragRepo.GetCrag(1);
        }
    }
}
