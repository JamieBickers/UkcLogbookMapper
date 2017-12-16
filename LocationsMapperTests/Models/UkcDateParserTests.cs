using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocationsMapper.Models;

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
    }
}
