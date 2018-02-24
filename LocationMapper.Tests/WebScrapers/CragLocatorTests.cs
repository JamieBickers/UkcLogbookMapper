using LocationMapper.Entities;
using LocationMapper.WebScrapers;
using LocationMapper.WebScrapers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace LocationMapper.Tests.WebScrapers
{
    [TestClass]
    public class CragLocatorTests
    {
        private CragLocator cragLocator;

        [TestMethod]
        public void TryFindCrag_CragExists_ExpectCragsLocation()
        {
            var mockSearchResult = new RootObject()
            {
                Results = new List<Result>()
                {
                    new Result()
                    {
                        Geometry = new Geometry()
                        {
                            Location = new Location()
                            {
                                Lat = 30,
                                Lng = 40
                            }
                        }
                    }
                }
            };

            var mockGoogleApi = new Mock<IGoogleApi>();
            mockGoogleApi
                .Setup(googleApi => googleApi.GoogleSearch(It.Is<IEnumerable<string>>(list => list.Count() == 1 && list.First() == "stanage")))
                .Returns(mockSearchResult);

            cragLocator = new CragLocator(mockGoogleApi.Object);

            var result = cragLocator.TryFindCrag("stanage", out var location);

            Assert.IsTrue(result);
            Assert.AreEqual(location.Latitude, 30);
            Assert.AreEqual(location.Longitude, 40);
        }

        [TestMethod]
        public void TryFindCrag_CragDoesNotExists_ExpectNotFound()
        {
            var mockSearchResult = new RootObject()
            {
                Results = new List<Result>()
                {
                    new Result()
                    {
                        Geometry = new Geometry()
                    }
                }
            };

            var mockGoogleApi = new Mock<IGoogleApi>();
            mockGoogleApi
                .Setup(googleApi => googleApi.GoogleSearch(It.Is<IEnumerable<string>>(list => list.Count() == 1 && list.First() == "stanage")))
                .Returns(mockSearchResult);

            cragLocator = new CragLocator(mockGoogleApi.Object);

            var result = cragLocator.TryFindCrag("stanage", out var location);

            Assert.IsFalse(result);
            Assert.IsNull(location);
        }

        [TestMethod]
        public void TryFindCrag_GoogleReturnsNullResult_ExpectNotFound()
        {
            var mockSearchResult = (RootObject)null;

            var mockGoogleApi = new Mock<IGoogleApi>();
            mockGoogleApi
                .Setup(googleApi => googleApi.GoogleSearch(It.Is<IEnumerable<string>>(list => list.Count() == 1 && list.First() == "stanage")))
                .Returns(mockSearchResult);

            cragLocator = new CragLocator(mockGoogleApi.Object);

            var result = cragLocator.TryFindCrag("stanage", out var location);

            Assert.IsFalse(result);
            Assert.IsNull(location);
        }

        [TestMethod]
        public void TryFindCragOverload_CragExists_ExpectCragsLocation()
        {
            var mockSearchResult = new RootObject()
            {
                Results = new List<Result>()
                {
                    new Result()
                    {
                        Geometry = new Geometry()
                        {
                            Location = new Location()
                            {
                                Lat = 30,
                                Lng = 40
                            }
                        }
                    }
                }
            };

            var mockGoogleApi = new Mock<IGoogleApi>();
            mockGoogleApi
                .Setup(googleApi => googleApi.GoogleSearch(It.Is<IEnumerable<string>>(
                    list => list.Count() == 2
                    && list.First() == "Oxfordshire"
                    && list.ElementAt(1) == "England")))
                .Returns(mockSearchResult);

            cragLocator = new CragLocator(mockGoogleApi.Object);

            var result = cragLocator.TryFindCrag("Oxfordshire", "England", out var location);

            Assert.IsTrue(result);
            Assert.AreEqual(location.Latitude, 30);
            Assert.AreEqual(location.Longitude, 40);
        }

        [TestMethod]
        public void TryFindCragOverload_CragDoesNotExists_ExpectNotFound()
        {
            var mockSearchResult = new RootObject()
            {
                Results = new List<Result>()
                {
                    new Result()
                    {
                        Geometry = new Geometry()
                    }
                }
            };

            var mockGoogleApi = new Mock<IGoogleApi>();
            mockGoogleApi
                .Setup(googleApi => googleApi.GoogleSearch(It.Is<IEnumerable<string>>(
                    list => list.Count() == 2
                    && list.First() == "Oxfordshire"
                    && list.ElementAt(1) == "England")))
                .Returns(mockSearchResult);

            cragLocator = new CragLocator(mockGoogleApi.Object);

            var result = cragLocator.TryFindCrag("Oxfordshire", "England", out var location);

            Assert.IsFalse(result);
            Assert.IsNull(location);
        }

        [TestMethod]
        public void TryFindCragOverload_GoogleReturnsNullResult_ExpectNotFound()
        {
            var mockSearchResult = (RootObject)null;

            var mockGoogleApi = new Mock<IGoogleApi>();
            mockGoogleApi
                .Setup(googleApi => googleApi.GoogleSearch(It.Is<IEnumerable<string>>(
                    list => list.Count() == 2
                    && list.First() == "Oxfordshire"
                    && list.ElementAt(1) == "England")))
                .Returns(mockSearchResult);

            cragLocator = new CragLocator(mockGoogleApi.Object);

            var result = cragLocator.TryFindCrag("Oxfordshire", "England", out var location);

            Assert.IsFalse(result);
            Assert.IsNull(location);
        }
    }
}
