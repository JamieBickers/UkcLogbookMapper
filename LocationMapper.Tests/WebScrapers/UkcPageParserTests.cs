using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using LocationMapper.WebScrapers;
using LocationMapper.Tests.WebScrapers.TestDocuments;
using System.Linq;
using LocationMapper.Entities;
using LocationMapper.WebScrapers.Entities;

namespace LocationMapper.Tests.WebScrapers
{
    [TestClass]
    public class UkcPageParserTests
    {
        private UkcPageParser pageParser;
        private DocumentReader documentReader;

        [TestInitialize]
        public void Init()
        {
            pageParser = new UkcPageParser();
            documentReader = new DocumentReader();
        }

        [TestMethod]
        public void TryGetAllClimbsOnPage_PageHasClimbs_ExpectClimbsOut()
        {
            var page = documentReader.ReadDocument("JmabUkcPage1.txt");

            var foundClimbs = pageParser.TryGetAllClimbsOnPage(page, out var climbs);

            Assert.IsTrue(foundClimbs);
            Assert.AreEqual(11, climbs.Count());
            Assert.IsTrue(climbs.Any(climb => climb.ClimbName == "All Fall Down"));
        }

        [TestMethod]
        public void TryGetAllClimbsOnPage_UserExistsButPageHasNoClimbs_ExpectNoClimbsOut()
        {
            var page = documentReader.ReadDocument("UserExistsButNoClimbsPage.txt");
            AssertNoClimbsFound(page);
        }

        [TestMethod]
        public void TryGetAllClimbsOnPage_UserDoesNotExist_ExpectNoClimbsOut()
        {
            var page = documentReader.ReadDocument("UserDoesNotExistPage.txt");
            AssertNoClimbsFound(page);
        }

        [TestMethod]
        public void TryGetAllClimbsOnPage_UseRandomWebPage_ExpectNoClimbsOut()
        {
            var page = documentReader.ReadDocument("BbcNewsHomePage.txt");
            AssertNoClimbsFound(page);
        }

        [TestMethod]
        public void TryGetAllClimbsOnPage_EmptyPage_ExpectNoClimbsOut()
        {
            var page = "";
            AssertNoClimbsFound(page);
        }

        [TestMethod]
        public void TryGetAllClimbsOnPage_NullPage_ExpectNoClimbsOut()
        {
            string page = null;
            AssertNoClimbsFound(page);
        }

        [TestMethod]
        public void TryGetCragInformationFromCragPage_Stanage_ExpectStanageInformation()
        {
            var page = documentReader.ReadDocument("StanageLogbookPage.txt");

            var readPageData = pageParser.TryGetCragInformationFromCragPage(page, out var crag);
            var expected = new UkcCrag()
            {
                CragName = "Stanage Popular",
                UkcCragId = 104,
                Location = new MapLocation()
                {
                    Longitude = -1.6319M,
                    Latitude = 53.3458M
                },
                Country = "England",
                County = "Derbyshire"
            };

            Assert.IsTrue(readPageData);
            Assert.AreEqual(expected.CragName, crag.CragName);
            Assert.AreEqual(expected.UkcCragId, crag.UkcCragId);
            Assert.AreEqual(expected.Location.Latitude, crag.Location.Latitude);
            Assert.AreEqual(expected.Location.Longitude, crag.Location.Longitude);
            Assert.AreEqual(expected.Country, crag.Country);
            Assert.AreEqual(expected.County, crag.County);
        }

        [TestMethod]
        public void TryGetCragInformationFromCragPage_CragNotFoundPage_ExpectLocationNotFound()
        {
            var page = documentReader.ReadDocument("CragNotFoundPage.txt");

            var foundInformation = pageParser.TryGetCragInformationFromCragPage(page, out var crag);

            Assert.IsFalse(foundInformation);
            Assert.AreEqual(null, crag);
        }

        [TestMethod]
        public void TryGetCragInformationFromCragPage_RandomWebPage_ExpectLocationNotFound()
        {
            var page = documentReader.ReadDocument("BbcNewsHomePage.txt");

            var foundInformation = pageParser.TryGetCragInformationFromCragPage(page, out var crag);

            Assert.IsFalse(foundInformation);
            Assert.AreEqual(null, crag);
        }

        [TestMethod]
        public void TryGetCragInformationFromCragPage_EmptyPage_ExpectLocationNotFound()
        {
            var page = "";

            var foundInformation = pageParser.TryGetCragInformationFromCragPage(page, out var crag);

            Assert.IsFalse(foundInformation);
            Assert.AreEqual(null, crag);
        }

        [TestMethod]
        public void TryGetCragInformationFromCragPage_NullPage_ExpectLocationNotFound()
        {
            string page = null;

            var foundInformation = pageParser.TryGetCragInformationFromCragPage(page, out var crag);

            Assert.IsFalse(foundInformation);
            Assert.AreEqual(null, crag);
        }

       [TestMethod]
        public void TryGetUserIdOnPage_PageHasUser_ExpectUserIdFound()
        {
            var page = documentReader.ReadDocument("JmabUserSearchPage.txt");

            var foundUserId = pageParser.TryGetUserIdOnSearchPage(page, out var userId);

            Assert.IsTrue(foundUserId);
            Assert.AreEqual("212307", userId);
        }

        [TestMethod]
        public void TryGetUserIdOnPage_PageHasNoUser_ExpectNotFound()
        {
            var page = documentReader.ReadDocument("UserNotFoundPage.txt");

            var foundUserId = pageParser.TryGetUserIdOnSearchPage(page, out var userId);

            Assert.IsFalse(foundUserId);
            Assert.AreEqual(null, userId);
        }

        [TestMethod]
        public void TryGetUserIdOnPage_PageHasMultipleUser_ExpectFirstFound()
        {
            var page = documentReader.ReadDocument("TwoUsersFoundSearchPage.txt");

            var foundUserId = pageParser.TryGetUserIdOnSearchPage(page, out var userId);

            Assert.IsTrue(foundUserId);
            Assert.AreEqual("78267", userId);
        }

        [TestMethod]
        public void TryGetUserIdOnPage_RandomWebPage_ExpectNoneFound()
        {
            var page = documentReader.ReadDocument("BbcNewsHomePage.txt");

            var foundUserId = pageParser.TryGetUserIdOnSearchPage(page, out var userId);

            Assert.IsFalse(foundUserId);
            Assert.AreEqual(null, userId);
        }

        [TestMethod]
        public void TryGetUserIdOnPage_EmptyPage_ExpectNotFound()
        {
            var page = "";

            var foundUserId = pageParser.TryGetUserIdOnSearchPage(page, out var userId);

            Assert.IsFalse(foundUserId);
            Assert.AreEqual(null, userId);
        }

        [TestMethod]
        public void TryGetUserIdOnPage_NullPage_ExpectNotFound()
        {
            string page = null;

            var foundUserId = pageParser.TryGetUserIdOnSearchPage(page, out var userId);

            Assert.IsFalse(foundUserId);
            Assert.AreEqual(null, userId);
        }

        private void AssertNoClimbsFound(string page)
        {
            var foundClimbs = pageParser.TryGetAllClimbsOnPage(page, out var climbs);
            Assert.IsFalse(foundClimbs);
            Assert.AreEqual(0, climbs?.Count() ?? 0);
        }
    }
}
