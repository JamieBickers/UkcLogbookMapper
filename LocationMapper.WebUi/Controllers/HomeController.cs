using Microsoft.AspNetCore.Mvc;
using LocationMapper.WebScrapers.Interfaces;
using LocationMapper.WebUi.ServiceLogic;
using System.Diagnostics;
using LocationMapper.WebUi.Models;
using System;
using Microsoft.Extensions.Options;

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

        public IActionResult MapName(string ukcUserName)
        {
            var locations = mapPlotter.FindLocationsUserHasClimbed(ukcUserName);
            return View("Map", locations);
        }

        public IActionResult MapId(int ukcUserId)
        {
            var locations = mapPlotter.FindLocationsUserHasClimbed(ukcUserId);
            return View("Map", locations);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult TryFindUser(string ukcUserName)
        {
            if (ukcReader.TryGetUserId(ukcUserName, out var userId))
            {
                return Ok(userId);
            }
            else
            {
                return StatusCode(404);
            }
        }
    }
}
