using LocationMapper.Repository;
using LocationMapper.WebScrapers;
using LocationMapper.WebUi.Controllers;
using LocationMapper.WebUi.ServiceLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace LocationMapper.Tests.IntegrationTests
{
    [TestClass]
    public class WholeSystemTests
    {
        private const string connectionString = "Host=localhost;Username=UkcLogbookMapper;Password=qwerty;Database=Ukc";
        [TestMethod]
        public void HomeControllerIndex_UsingDefaultConstructorArguments_ExpectNoExceptionsThrownAndNotNull()
        {
            var controller = new HomeController(new UkcReader(), new CragLocator());
            var result = controller.Index();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HomeControllerTryFindUser_UserExists_ExpectNoExceptionsThrownAndNotNull()
        {
            var controller = new HomeController(new UkcReader(), new CragLocator());
            var result = controller.TryFindUser("jmab");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HomeControllerTryFindUser_UserDoesNotExist_ExpectNoExceptionsThrownAndNotNull()
        {
            var controller = new HomeController(new UkcReader(), new CragLocator());
            var result = controller.TryFindUser("ghdehyerdgfashdf");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HomeControllerMap_UsingExistingUser_ExpectNoExceptionsThrownAndNotNull()
        {
            var controller = new HomeController(new UkcReader(), new CragLocator());
            var result = controller.MapName("jmab");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HomeControllerMap_UserDoesNotExist_ExpectNoException()
        {
            var controller = new HomeController(new UkcReader(), new CragLocator());
            var result = controller.MapName("dfsdfsdfsghidsgfhjdsfds");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FindLocationsUserHasClimbed_ExistingUserWithClimbs_ExpectClimbsFound()
        {
            var mapPlotter = new MapPlotter(new UkcReader(), CragRepositoryFactory.GetCragRepository(connectionString), new CragLocator());
            var result = mapPlotter.FindLocationsUserHasClimbed("jmab");

            var stanageLocation = result
                .First(marker => marker.ClimbName == "Original Sloper Traverse/Northeast Face")
                .Location;

            Assert.IsTrue(result.Count() > 10);
            Assert.AreEqual(stanageLocation.Latitude, 53.347292m);
            Assert.AreEqual(stanageLocation.Longitude, -1.633261m);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void FindLocationsUserHasClimbed_ExistingUserWithPrivateLogbook_ExpectFourOhFour()
        {
            var mapPlotter = new MapPlotter(new UkcReader(), CragRepositoryFactory.GetCragRepository(connectionString), new CragLocator());
            var result = mapPlotter.FindLocationsUserHasClimbed("GilesW");
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void FindLocationsUserHasClimbed_ExistingUserWithNoClimbs_ExpectNoClimbsFound()
        {
            var mapPlotter = new MapPlotter(new UkcReader(), CragRepositoryFactory.GetCragRepository(connectionString), new CragLocator());
            var result = mapPlotter.FindLocationsUserHasClimbed("charlycalles");

            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void TryFindUser_ExistingUser_ExpectUserIdReturned()
        {
            var ukcReader = new UkcReader();
            var result = ukcReader.TryGetUserId("jmab", out var userId);

            Assert.IsTrue(result);
            Assert.AreEqual(212307, userId);
        }

        [TestMethod]
        public void TryFindUser_NotExistingUser_ExpectNoUserIdReturned()
        {
            var ukcReader = new UkcReader();
            var result = ukcReader.TryGetUserId("zashdfhrtttd", out var userId);

            Assert.IsFalse(result);
            Assert.AreEqual(0, userId);
        }
    }
}