using LocationMapper.Entities;
using LocationMapper.WebScrapers.Interfaces;
using LocationMapper.WebUi.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LocationMapper.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LocationMapper.WebUi.ServiceLogic
{
    class MapPlotter
    {
        private ICragLocator cragLocator;
        private IUkcReader ukcReader;

        public MapPlotter(ICragLocator cragLocator, IUkcReader ukcReader)
        {
            this.cragLocator = cragLocator;
            this.ukcReader = ukcReader;
        }

        public IEnumerable<MapMarkerDto> FindLocationsUserHasClimbed(int userId)
        {
            var climbs = ukcReader.GetAllClimbs(userId);
            var mapMarkers = new List<MapMarkerDto>();

            foreach (var climb in climbs)
            {
                if (TryFindCragLocation(climb.CragName, climb.CragId, out var location))
                {
                    mapMarkers.Add(new MapMarkerDto()
                    {
                        ClimbName = climb.ClimbName,
                        Grade = climb.Grade,
                        Location = location
                    });
                }
            }

            return mapMarkers;
        }

        public IEnumerable<MapMarkerDto> FindLocationsUserHasClimbed(string userName)
        {
            if (ukcReader.TryGetUserId(userName, out var userId))
            {
                return FindLocationsUserHasClimbed(userId);
            }
            else
            {
                return null;
            }
        }

        private bool TryFindCragLocation(string cragName, int cragId, out MapLocation location)
        {
            if (cragLocator.TryFindCrag(cragName, out location))
            {
                return true;
            }
            else
            {
                var (County, Country) = ukcReader.GetRoughCragLocation(cragId);
                if (cragLocator.TryFindCrag(County, Country, out location))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
