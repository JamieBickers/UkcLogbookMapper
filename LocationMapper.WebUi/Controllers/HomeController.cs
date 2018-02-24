using Microsoft.AspNetCore.Mvc;
using LocationMapper.WebScrapers.Interfaces;
using LocationMapper.WebUi.ServiceLogic;
using System.Diagnostics;
using LocationMapper.WebUi.Models;

namespace LocationMapper.WebUi.Controllers
{
    public class HomeController : Controller
    {
        private IUkcReader ukcReader;
        private ICragLocator cragLocator;
        private MapPlotter mapPlotter;

        public HomeController(IUkcReader ukcReader, ICragLocator cragLocator)
        {
            this.ukcReader = ukcReader;
            this.cragLocator = cragLocator;

            mapPlotter = new MapPlotter(cragLocator, ukcReader);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Map(string ukcUserName)
        {
            var locations = mapPlotter.FindLocationsUserHasClimbed(ukcUserName);
            return View(locations);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
