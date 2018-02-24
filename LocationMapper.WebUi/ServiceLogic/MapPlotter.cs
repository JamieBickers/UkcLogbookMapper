using LocationMapper.Entities;
using LocationMapper.Models;
using LocationMapper.WebScrapers.Interfaces;
using LocationMapper.WebUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public IEnumerable<MapMarkerDto> FindLocationsUserHasClimbed(string userName)
        {
            var climbs = ukcReader.GetAllClimbs(userName);
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
