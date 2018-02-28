using LocationMapper.Entities;
using LocationMapper.Repository;
using LocationMapper.WebScrapers;
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

            var ukcReader = new UkcReader();

            var crag = cragRepository.GetCrag(1);
            cragRepository.AddCrag(new Crag()
            {
                CragName = "Burbage",
                UkcCragId = 2,
                ExactLocation = false,
                ID = 4,
                Latitude = 10,
                Longitude = 200
            });
            crag = cragRepository.GetCrag(2);
        }
    }
}
