using LocationsMapper.Database;
using LocationsMapper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationMapperTests.Models
{
    [TestClass]
    public class CragLocatorTests
    {
        [TestMethod]
        public void CheckFindsStanage()
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=CragLocations;Trusted_Connection=True;ConnectRetryCount=0";
            var options = new DbContextOptionsBuilder<CragLocationContext>().UseSqlServer(connection).Options;
            var context = new CragLocationContext(options);

            var computedLocation = CragLocator.FindCrag("Stanage Planation", 101, context);
            var stanagesLocation = new MapLocation(53.347292m, -1.633261m);

            Assert.AreEqual(stanagesLocation.Latitude, computedLocation.Latitude);
            Assert.AreEqual(stanagesLocation.Longitude, computedLocation.Longitude);
        }
    }
}
