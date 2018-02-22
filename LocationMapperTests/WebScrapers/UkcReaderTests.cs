using LocationMapper.WebScrapers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocationMapperTests.WebScrapers
{
    [TestClass]
    public class UkcReaderTests
    {
        private UkcReader ukcReader;

        [TestInitialize]
        public void Init()
        {
            ukcReader = new UkcReader();
        }

        [TestMethod]
        public void ReadFromSinglePageTest()
        {
            var userName = "daftendirekt";

            var climbs = ukcReader.GetAllClimbs(userName);
        }
    }
}
