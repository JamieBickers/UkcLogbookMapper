using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocationsMapper.Models;
using LocationsMapper.WebScrapers;

namespace LocationsMapperTests
{
    [TestClass]
    public class UkcDateParserTests
    {
        [TestMethod]
        public void KnownThisYear()
        {
            var date = "12/May";

            var parsedDate = UkcDateParser.CalculateDate(date);

            Assert.AreEqual(parsedDate.Year, DateTimeOffset.Now.Year);
            Assert.AreEqual(parsedDate.Month, 5);
            Assert.AreEqual(parsedDate.Day, 12);
        }

        [TestMethod]
        public void KnownNotThisYear()
        {
            var date = "3/Sep/14";

            var parsedDate = UkcDateParser.CalculateDate(date);

            Assert.AreEqual(parsedDate.Year, 2014);
            Assert.AreEqual(parsedDate.Month, 9);
            Assert.AreEqual(parsedDate.Day, 3);
        }

        [TestMethod]
        public void UnknownMonthThisYear()
        {
            var date = "??";

            var parsedDate = UkcDateParser.CalculateDate(date);

            Assert.AreEqual(parsedDate.Year, DateTimeOffset.Now.Year);
            Assert.AreEqual(parsedDate.Month, 6);
            Assert.AreEqual(parsedDate.Day, 15);
        }

        [TestMethod]
        public void UnknownMonthNotThisYear()
        {
            var date = "??/12";

            var parsedDate = UkcDateParser.CalculateDate(date);

            Assert.AreEqual(parsedDate.Year, 12);
            Assert.AreEqual(parsedDate.Month, 6);
            Assert.AreEqual(parsedDate.Day, 15);
        }

        [TestMethod]
        public void UnknownDayThisYear()
        {
            var date = "?/Apr";

            var parsedDate = UkcDateParser.CalculateDate(date);

            Assert.AreEqual(parsedDate.Year, DateTimeOffset.Now.Year);
            Assert.AreEqual(parsedDate.Month, 4);
            Assert.AreEqual(parsedDate.Day, 15);
        }

        [TestMethod]
        public void UnknownDayNotThisYear()
        {
            var date = "?/Aug/9";

            var parsedDate = UkcDateParser.CalculateDate(date);

            Assert.AreEqual(parsedDate.Year, 9);
            Assert.AreEqual(parsedDate.Month, 8);
            Assert.AreEqual(parsedDate.Day, 15);
        }
    }
}
