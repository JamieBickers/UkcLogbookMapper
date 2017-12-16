using LocationsMapper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationsMapper.Database
{
    public class DatabaseQueries
    {
        private readonly CragLocationContext context;

        public DatabaseQueries(CragLocationContext context)
        {
            this.context = context;
        }

        public bool TryGetLocationFromDatabase(int ukcCragId, out MapLocation location)
        {
            var possibleLocations = context.CragLocations.Where(crag => crag.UkcCragId == ukcCragId);
            if (!possibleLocations.Any())
            {
                location = new MapLocation(0, 0);
                return false;
            }
            else
            {
                var cragLocation = possibleLocations.First();
                location = new MapLocation(cragLocation.Latitude, cragLocation.Longitude);
                return true;
            }
        }

        public void AddCragToDatabase(int ukcCragId, string cragName, decimal latitude, decimal longitude, bool exactLocation)
        {
            context.CragLocations.Add(new CragLocation()
            {
                CragName = cragName,
                UkcCragId = ukcCragId,
                Latitude = latitude,
                Longitude = longitude,
                ExactLocation = exactLocation
            });
            context.SaveChanges();
        }
    }
}
