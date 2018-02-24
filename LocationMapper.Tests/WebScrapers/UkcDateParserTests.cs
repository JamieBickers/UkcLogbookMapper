using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocationsMapper.WebScrapers;

namespace LocationsMapper.Tests
{
    [TestClass]
    public class UkcDateParserTests
    {
        [TestMethod]
        public void CalculateDate_KnownThisYear_ExpectDate()
        {
            var date = "12/May";

            var parsedDate = date.DeserialiseUkcFormattedDate();

            Assert.AreEqual(parsedDate.Year, DateTimeOffset.Now.Year);
            Assert.AreEqual(parsedDate.Month, 5);
            Assert.AreEqual(parsedDate.Day, 12);
        }

        [TestMethod]
        public void CalculateDate_KnownNotThisYear_ExpectDate()
        {
            var date = "3/Sep/14";

            var parsedDate = date.DeserialiseUkcFormattedDate();

            Assert.AreEqual(parsedDate.Year, 2014);
            Assert.AreEqual(parsedDate.Month, 9);
            Assert.AreEqual(parsedDate.Day, 3);
        }

        [TestMethod]
        public void CalculateDate_UnknownMonthThisYear_ExpectCorrectMonthAndDayAndThisYear()
        {
            var date = "??";

            var parsedDate = date.DeserialiseUkcFormattedDate();

            Assert.AreEqual(parsedDate.Year, DateTimeOffset.Now.Year);
            Assert.AreEqual(parsedDate.Month, 6);
            Assert.AreEqual(parsedDate.Day, 15);
        }

        [TestMethod]
        public void CalculateDate_UnknownMonthNotThisYear_ExpectDateWithGuessedMonth()
        {
            var date = "??/12";

            var parsedDate = date.DeserialiseUkcFormattedDate();

            Assert.AreEqual(parsedDate.Year, 12);
            Assert.AreEqual(parsedDate.Month, 6);
            Assert.AreEqual(parsedDate.Day, 15);
        }

        [TestMethod]
        public void CalculateDate_UnknownDayThisYear_ExpectDateWithGuessedDay()
        {
            var date = "?/Apr";

            var parsedDate = date.DeserialiseUkcFormattedDate();

            Assert.AreEqual(parsedDate.Year, DateTimeOffset.Now.Year);
            Assert.AreEqual(parsedDate.Month, 4);
            Assert.AreEqual(parsedDate.Day, 15);
        }

        [TestMethod]
        public void CalculateDate_UnknownDayNotThisYear_ExpectDateWithGuessedMonth()
        {
            var date = "?/Aug/9";

            var parsedDate = date.DeserialiseUkcFormattedDate();

            Assert.AreEqual(parsedDate.Year, 9);
            Assert.AreEqual(parsedDate.Month, 8);
            Assert.AreEqual(parsedDate.Day, 15);
        }
    }
}
