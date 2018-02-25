using LocationMapper.Entities;
using LocationMapper.WebScrapers.Interfaces;
using LocationMapper.WebUi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LocationMapper.WebUi.ServiceLogic;

namespace LocationMapper.Tests.WebUi.ServiceLogic
{
    [TestClass]
    public class MapPlotterTests
    {
        private MapPlotter mapPlotter;

        [TestMethod]
        public void FindLocationsUserHasClimbed_ClimbsCanBeFoundExactly_ExpectClimbsFound()
        {
            var userId = 212307;

            var mapLocation1 = new MapLocation()
            {
                Latitude = 10,
                Longitude = 20
            };

            var mapLocation2 = new MapLocation()
            {
                Latitude = 30,
                Longitude = 40
            };

            var mockUkcReader = new Mock<IUkcReader>();
            mockUkcReader
                .Setup(ukcReader => ukcReader.GetAllClimbs(212307))
                .Returns(new List<LogbookEntry>()
                {
                    new LogbookEntry()
                    {
                        CragName = "stanage",
                        CragId = 1
                    },
                    new LogbookEntry()
                    {
                        CragName = "burbage",
                        CragId = 2
                    }
                });

            mockUkcReader
                .Setup(ukcReader => ukcReader.TryGetUserId("jmab", out userId))
                .Returns(true);

            var mockCragLocator = new Mock<ICragLocator>();
            mockCragLocator
                .Setup(cragLocator => cragLocator.TryFindCrag("stanage", out mapLocation1))
                .Returns(true);

            mockCragLocator
                .Setup(cragLocator => cragLocator.TryFindCrag("burbage", out mapLocation2))
                .Returns(true);

            mapPlotter = new MapPlotter(mockCragLocator.Object, mockUkcReader.Object);

            var result = mapPlotter.FindLocationsUserHasClimbed("jmab");

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(10, result.First().Location.Latitude);
            Assert.AreEqual(40, result.ElementAt(1).Location.Longitude);
        }

        [TestMethod]
        public void FindLocationsUserHasClimbed_ClimbsCannotBeFoundExactly_ExpectClimbsFound()
        {
            var userId = 212307;

            var mapLocation1 = new MapLocation()
            {
                Latitude = 10,
                Longitude = 20
            };

            var mapLocation2 = new MapLocation()
            {
                Latitude = 30,
                Longitude = 40
            };

            var mockUkcReader = new Mock<IUkcReader>();
            mockUkcReader
                .Setup(ukcReader => ukcReader.GetAllClimbs(212307))
                .Returns(new List<LogbookEntry>()
                {
                    new LogbookEntry()
                    {
                        CragName = "stanage",
                        CragId = 1
                    },
                    new LogbookEntry()
                    {
                        CragName = "burbage",
                        CragId = 2
                    }
                });

            mockUkcReader
                .Setup(ukcReader => ukcReader.GetRoughCragLocation(2))
                .Returns(("Oxfordshire", "England"));

            mockUkcReader
                .Setup(ukcReader => ukcReader.TryGetUserId("jmab", out userId))
                .Returns(true);

            var mockCragLocator = new Mock<ICragLocator>();
            mockCragLocator
                .Setup(cragLocator => cragLocator.TryFindCrag("stanage", out mapLocation1))
                .Returns(true);

            mockCragLocator
                .Setup(cragLocator => cragLocator.TryFindCrag("burbage", out mapLocation2))
                .Returns(false);

            mockCragLocator
                .Setup(cragLocator => cragLocator.TryFindCrag("Oxfordshire", "England", out mapLocation2))
                .Returns(true);

            mapPlotter = new MapPlotter(mockCragLocator.Object, mockUkcReader.Object);

            var result = mapPlotter.FindLocationsUserHasClimbed("jmab");

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(10, result.First().Location.Latitude);
            Assert.AreEqual(40, result.ElementAt(1).Location.Longitude);
        }

        [TestMethod]
        public void FindLocationsUserHasClimbed_ClimbCannotBeFoundOnSearch_ExpectClimbNotFound()
        {
            var mapLocation1 = new MapLocation();

            var mockUkcReader = new Mock<IUkcReader>();
            mockUkcReader
                .Setup(ukcReader => ukcReader.GetAllClimbs(212307))
                .Returns(new List<LogbookEntry>()
                {
                    new LogbookEntry()
                    {
                        CragName = "stanage",
                        CragId = 1
                    }
                });

            mockUkcReader
                .Setup(ukcReader => ukcReader.GetRoughCragLocation(1))
                .Returns(((string)null, (string)null));

            var mockCragLocator = new Mock<ICragLocator>();
            mockCragLocator
                .Setup(cragLocator => cragLocator.TryFindCrag("stanage", out mapLocation1))
                .Returns(false);

            mapPlotter = new MapPlotter(mockCragLocator.Object, mockUkcReader.Object);

            var result = mapPlotter.FindLocationsUserHasClimbed("jmab");

            Assert.AreEqual(0, result?.Count() ?? 0);
        }
    }
}
