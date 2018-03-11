using LocationMapper.Entities;
using LocationMapper.Tests.WebScrapers.TestDocuments;
using LocationMapper.WebScrapers;
using LocationMapper.WebScrapers.Entities;
using LocationMapper.WebScrapers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationMapper.Tests.WebScrapers
{
    [TestClass]
    public class UkcReaderTests
    {
        private IUkcReader ukcReader;
        private DocumentReader documentReader;

        [TestInitialize]
        public void Init()
        {
            documentReader = new DocumentReader();
        }

        [TestMethod]
        public void GetAllClimbs_UserWithOnePage_ExpectToRetreiveCimbs()
        {
            var jmabUkcLogbookpage = documentReader.ReadDocument("JmabUkcPage1.txt");
            var jmabUkcUserSearchPage = documentReader.ReadDocument("JmabUserSearchPage.txt");

            IEnumerable<LogbookEntry> climbs = new List<LogbookEntry>()
            {
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry()
            };
            var userId = "212307";

            var mockPageReader = new Mock<IUkcPageReader>();
            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, It.IsAny<int>()))
                .Returns(jmabUkcLogbookpage);

            mockPageReader
                .Setup(pageReader => pageReader.GetSearchPage("jmab"))
                .Returns(jmabUkcUserSearchPage);

            var mockPageParser = new Mock<IUkcPageParser>();
            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(jmabUkcLogbookpage, out climbs))
                .Returns(true);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetUserIdOnSearchPage(jmabUkcUserSearchPage, out userId))
                .Returns(true);

            ukcReader = new UkcReader(mockPageReader.Object, mockPageParser.Object);

            var actualClimbs = ukcReader.GetAllClimbs(212307);

            Assert.AreEqual(3, actualClimbs.Count());
        }

        [TestMethod]
        public void GetAllClimbs_UserWithMultiplePages_ExpectToRetreiveCimbs()
        {
            var logbookPage1 = "p1";
            var logbookPage2 = "p2";
            var jmabUkcUserSearchPage = documentReader.ReadDocument("JmabUserSearchPage.txt");

            IEnumerable<LogbookEntry> climbs1 = new List<LogbookEntry>()
            {
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry()
            };

            IEnumerable<LogbookEntry> climbs2 = new List<LogbookEntry>()
            {
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry()
            };

            var userId = "212307";

            var mockPageReader = new Mock<IUkcPageReader>();
            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, 1))
                .Returns(logbookPage1);

            mockPageReader
            .Setup(pageReader => pageReader.GetUserLogbookPage(212307, 2))
            .Returns(logbookPage2);

            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, It.Is<int>(number => number > 2)))
                .Returns(logbookPage1);

            mockPageReader
                .Setup(pageReader => pageReader.GetSearchPage("jmab"))
                .Returns(jmabUkcUserSearchPage);

            var mockPageParser = new Mock<IUkcPageParser>();
            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage1, out climbs1))
                .Returns(true);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage2, out climbs2))
                .Returns(true);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetUserIdOnSearchPage(jmabUkcUserSearchPage, out userId))
                .Returns(true);

            ukcReader = new UkcReader(mockPageReader.Object, mockPageParser.Object);

            var actualClimbs = ukcReader.GetAllClimbs(212307);

            Assert.AreEqual(7, actualClimbs.Count());
        }

        [TestMethod]
        public void GetAllClimbs_UserWithMultiplePagesAndExtraPagesAreRandom_ExpectToRetreiveCimbs()
        {
            var logbookPage1 = "p1";
            var logbookPage2 = "p2";
            var logbookPage3 = "p3";
            var jmabUkcUserSearchPage = documentReader.ReadDocument("JmabUserSearchPage.txt");

            IEnumerable<LogbookEntry> climbs1 = new List<LogbookEntry>()
            {
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry()
            };

            IEnumerable<LogbookEntry> climbs2 = new List<LogbookEntry>()
            {
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry()
            };

            IEnumerable<LogbookEntry> climbs3 = null;

            var userId = "212307";

            var mockPageReader = new Mock<IUkcPageReader>();
            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, 1))
                .Returns(logbookPage1);

            mockPageReader
            .Setup(pageReader => pageReader.GetUserLogbookPage(212307, 2))
            .Returns(logbookPage2);

            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, It.Is<int>(number => number > 2)))
                .Returns(logbookPage3);

            mockPageReader
                .Setup(pageReader => pageReader.GetSearchPage("jmab"))
                .Returns(jmabUkcUserSearchPage);

            var mockPageParser = new Mock<IUkcPageParser>();
            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage1, out climbs1))
                .Returns(true);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage2, out climbs2))
                .Returns(true);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage3, out climbs3))
                .Returns(false);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetUserIdOnSearchPage(jmabUkcUserSearchPage, out userId))
                .Returns(true);

            ukcReader = new UkcReader(mockPageReader.Object, mockPageParser.Object);

            var actualClimbs = ukcReader.GetAllClimbs(212307);

            Assert.AreEqual(7, actualClimbs.Count());
        }

        [TestMethod]
        public void GetAllClimbs_UserWithMultiplePagesAndExtraPagesReturnFalseAndClimbs_ExpectToRetreiveCimbsWithoutExtras()
        {
            var logbookPage1 = "p1";
            var logbookPage2 = "p2";
            var logbookPage3 = "p3";
            var jmabUkcUserSearchPage = documentReader.ReadDocument("JmabUserSearchPage.txt");

            IEnumerable<LogbookEntry> climbs1 = new List<LogbookEntry>()
            {
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry()
            };

            IEnumerable<LogbookEntry> climbs2 = new List<LogbookEntry>()
            {
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry(),
                new LogbookEntry()
            };

            IEnumerable<LogbookEntry> climbs3 = new List<LogbookEntry>()
            {
                new LogbookEntry()
            };

            var userId = "212307";

            var mockPageReader = new Mock<IUkcPageReader>();
            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, 1))
                .Returns(logbookPage1);

            mockPageReader
            .Setup(pageReader => pageReader.GetUserLogbookPage(212307, 2))
            .Returns(logbookPage2);

            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, It.Is<int>(number => number > 2)))
                .Returns(logbookPage3);

            mockPageReader
                .Setup(pageReader => pageReader.GetSearchPage("jmab"))
                .Returns(jmabUkcUserSearchPage);

            var mockPageParser = new Mock<IUkcPageParser>();
            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage1, out climbs1))
                .Returns(true);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage2, out climbs2))
                .Returns(true);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage3, out climbs3))
                .Returns(false);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetUserIdOnSearchPage(jmabUkcUserSearchPage, out userId))
                .Returns(true);

            ukcReader = new UkcReader(mockPageReader.Object, mockPageParser.Object);

            var actualClimbs = ukcReader.GetAllClimbs(212307);

            Assert.AreEqual(7, actualClimbs.Count());
        }

        [TestMethod]
        public void GetAllClimbs_UserNotFound_ExpectNoClimbs()
        {
            var logbookPage1 = "p1";
            var logbookPage2 = "p2";
            var jmabUkcUserSearchPage = documentReader.ReadDocument("JmabUserSearchPage.txt");

            string userId = null;

            IEnumerable<LogbookEntry> climbs1 = null;
            IEnumerable<LogbookEntry> climbs2 = null;

            var mockPageReader = new Mock<IUkcPageReader>();
            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, 1))
                .Returns(logbookPage1);

            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, It.Is<int>(number => number > 1)))
                .Returns(logbookPage2);

            mockPageReader
                .Setup(pageReader => pageReader.GetSearchPage("jmab"))
                .Returns(jmabUkcUserSearchPage);

            var mockPageParser = new Mock<IUkcPageParser>();
            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage1, out climbs1))
                .Returns(false);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage2, out climbs2))
                .Returns(false);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetUserIdOnSearchPage(jmabUkcUserSearchPage, out userId))
                .Returns(false);

            ukcReader = new UkcReader(mockPageReader.Object, mockPageParser.Object);

            var actualClimbs = ukcReader.GetAllClimbs(212307);

            Assert.AreEqual(0, actualClimbs?.Count() ?? 0);
        }

        [TestMethod]
        public void GetAllClimbs_userHasNoClimbs_ExpectToRetreiveZeroClimbs()
        {
            var logbookPage1 = "p1";
            var logbookPage2 = "p2";
            var jmabUkcUserSearchPage = documentReader.ReadDocument("JmabUserSearchPage.txt");

            IEnumerable<LogbookEntry> climbs1 = new List<LogbookEntry>();

            IEnumerable<LogbookEntry> climbs2 = null;

            var userId = "212307";

            var mockPageReader = new Mock<IUkcPageReader>();
            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, 1))
                .Returns(logbookPage1);

            mockPageReader
                .Setup(pageReader => pageReader.GetUserLogbookPage(212307, It.Is<int>(number => number > 1)))
                .Returns(logbookPage2);

            mockPageReader
                .Setup(pageReader => pageReader.GetSearchPage("jmab"))
                .Returns(jmabUkcUserSearchPage);

            var mockPageParser = new Mock<IUkcPageParser>();
            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage1, out climbs1))
                .Returns(true);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetAllClimbsOnPage(logbookPage2, out climbs2))
                .Returns(false);

            mockPageParser
                .Setup(pageParser => pageParser.TryGetUserIdOnSearchPage(jmabUkcUserSearchPage, out userId))
                .Returns(true);

            ukcReader = new UkcReader(mockPageReader.Object, mockPageParser.Object);

            var actualClimbs = ukcReader.GetAllClimbs(212307);

            Assert.AreEqual(0, actualClimbs.Count());
        }

        [TestMethod]
        public void GetCragData_ExistingCrag_ExpectCragLocationReturned()
        {
            var cragPage = documentReader.ReadDocument("StanageLogbookPage.txt");

            var location = new UkcCrag()
            {
                Country = "England",
                County = "Derbyshire",
                CragName = "Stanage Popular",
                UkcCragId = 104
            };

            var mockPageReader = new Mock<IUkcPageReader>();
            mockPageReader
                .Setup(pageReader => pageReader.GetCragPage(104))
                .Returns(cragPage);

            var mockPageParser = new Mock<IUkcPageParser>();
            mockPageParser
                .Setup(pageParser => pageParser.TryGetCragInformationFromCragPage(cragPage, out location))
                .Returns(true);

            ukcReader = new UkcReader(mockPageReader.Object, mockPageParser.Object);

            var actualLocation = ukcReader.GetCragData(104);

            Assert.AreEqual(location, actualLocation);
        }

        [TestMethod]
        public void GetCragData_CragDoesNotExist_ExpectNull()
        {
            var cragPage = documentReader.ReadDocument("StanageLogbookPage.txt");

            UkcCrag location = null;

            var mockPageReader = new Mock<IUkcPageReader>();
            mockPageReader
                .Setup(pageReader => pageReader.GetCragPage(104))
                .Returns(cragPage);

            var mockPageParser = new Mock<IUkcPageParser>();
            mockPageParser
                .Setup(pageParser => pageParser.TryGetCragInformationFromCragPage(cragPage, out location))
                .Returns(true);

            ukcReader = new UkcReader(mockPageReader.Object, mockPageParser.Object);

            var actualLocation = ukcReader.GetCragData(104);

            Assert.AreEqual(location, actualLocation);
        }

        [TestMethod]
        public void TryGetuserId_UserIsFound_ExpectUserIdReturned()
        {
            var userIdAsString = "212307";

            var pageReader = new Mock<IUkcPageReader>();
            pageReader
                .Setup(reader => reader.GetSearchPage("jmab"))
                .Returns("somePage");

            var pageParser = new Mock<IUkcPageParser>();
            pageParser
                .Setup(parser => parser.TryGetUserIdOnSearchPage("somePage", out userIdAsString))
                .Returns(true);

            ukcReader = new UkcReader(pageReader.Object, pageParser.Object);
            var result = ukcReader.TryGetUserId("jmab", out var userId);

            Assert.IsTrue(result);
            Assert.AreEqual(212307, userId);
        }

        [TestMethod]
        public void TryGetuserId_UserIsNotFound_ExpectNotFound()
        {
            var userIdAsString = (string)null;

            var pageReader = new Mock<IUkcPageReader>();
            pageReader
                .Setup(reader => reader.GetSearchPage("jmab"))
                .Returns("somePage");

            var pageParser = new Mock<IUkcPageParser>();
            pageParser
                .Setup(parser => parser.TryGetUserIdOnSearchPage("somePage", out userIdAsString))
                .Returns(false);

            ukcReader = new UkcReader(pageReader.Object, pageParser.Object);
            var result = ukcReader.TryGetUserId("jmab", out var userId);

            Assert.IsFalse(result);
            Assert.AreEqual(0, userId);
        }
    }
}
