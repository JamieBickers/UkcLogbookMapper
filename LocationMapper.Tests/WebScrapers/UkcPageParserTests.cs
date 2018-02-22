﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using LocationMapper.WebScrapers;
using LocationMapper.Tests.WebScrapers.TestDocuments;
using System.Linq;
using LocationMapper.Entities;

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
        public void TryGetRoughCragLocation_StanagePage_ExpectStanagesLocation()
        {
            var page = documentReader.ReadDocument("StanageLogbookPage.txt");

            var foundLocation = pageParser.TryGetRoughCragLocation(page, out var location);

            Assert.IsTrue(foundLocation);
            Assert.AreEqual(("Derbyshire", "England"), location);
        }

        [TestMethod]
        public void TryGetRoughCragLocation_CragNotFoundPage_ExpectLocationNtFound()
        {
            var page = documentReader.ReadDocument("CragNotFoundPage.txt");

            var foundLocation = pageParser.TryGetRoughCragLocation(page, out var location);

            Assert.IsFalse(foundLocation);
            Assert.AreEqual((null, null), location);
        }

        [TestMethod]
        public void TryGetRoughCragLocation_RandomWebPage_ExpectLocationNtFound()
        {
            var page = documentReader.ReadDocument("BbcNewsHomePage.txt");

            var foundLocation = pageParser.TryGetRoughCragLocation(page, out var location);

            Assert.IsFalse(foundLocation);
            Assert.AreEqual((null, null), location);
        }

        [TestMethod]
        public void TryGetUserIdOnPage_PageHasUser_ExpectUserIdFound()
        {
            var page = documentReader.ReadDocument("JmabUserSearchPage.txt");

            var foundUserId = pageParser.TryGetUserIdOnSearchPage(page, out var userId);

            Assert.IsTrue(foundUserId);
            Assert.AreEqual("212307", userId);
        }

        private void AssertNoClimbsFound(string page)
        {
            var foundClimbs = pageParser.TryGetAllClimbsOnPage(page, out var climbs);
            Assert.IsFalse(foundClimbs);
            Assert.AreEqual(0, climbs?.Count() ?? 0);
        }
    }
}