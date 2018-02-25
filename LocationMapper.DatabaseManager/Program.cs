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
            var connectionString = "Host=localhost;Username=UkcLogbookMapper;Password=qwerty;Database=Ukc";
            var cragRepository = CragRepositoryFactory.GetCragRepository(connectionString);

            var crag = cragRepository.GetCrag(1);
        }
    }
}
