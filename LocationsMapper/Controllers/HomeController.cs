using System.Linq;
using Microsoft.AspNetCore.Mvc;
using LocationsMapper.Models;
using System.Collections.Generic;
using LocationsMapper.Database;

namespace LocationsMapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly CragLocationContext context;
        public HomeController(CragLocationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Map(string userName)
        {
            var ukcUserId = WebScraper.GetUserId(userName);
            var climbs = WebScraper.GetAllClimbs(ukcUserId);

            var fromDatabase = context.CragLocations.Select(entry => new
            {
                CragId = entry.UkcCragId,
                Latitude = entry.Latitude,
                Longitude = entry.Longitude
            });

            var preComputedLocations = fromDatabase
                .GroupBy(entry => entry.CragId)
                .Select(group => group.First())
                .Join(climbs,
                entry => entry.CragId,
                climb => climb.CragId,
                (entry, climb) => entry)
                .ToList();

            var mapMarkers = new List<MapMarkerDto>();
            var preComputedCrags = preComputedLocations.Select(location => location.CragId);

            foreach (var climb in climbs)
            {
                if (preComputedCrags.Contains(climb.CragId))
                {
                    var location = preComputedLocations.First(entry => entry.CragId == climb.CragId);
                    var locationAsObject = new MapLocation(location.Latitude, location.Longitude);
                    mapMarkers.Add(new MapMarkerDto()
                    {
                        ClimbName = climb.ClimbName,
                        Grade = climb.Grade,
                        Location = locationAsObject
                    });
                }
                else
                {
                    var location = CragLocator.FindCrag(climb.CragName, climb.CragId, context);
                    mapMarkers.Add(new MapMarkerDto()
                    {
                        ClimbName = climb.ClimbName,
                        Grade = climb.Grade,
                        Location = location
                    });
                }
            }

            return View(mapMarkers);
        }
    }
}
