using LocationMapper.Entities;
using LocationMapper.Repository;
using LocationMapper.WebScrapers.Entities;
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
        private IUkcReader ukcReader;
        private ICragRepository cragRepository;
        private ICragLocator cragLocator;

        public MapPlotter(IUkcReader ukcReader, ICragRepository cragRepository, ICragLocator cragLocator)
        {
            this.ukcReader = ukcReader;
            this.cragRepository = cragRepository;
            this.cragLocator = cragLocator;
        }

        public IEnumerable<MapMarkerDto> FindLocationsUserHasClimbed(int userId)
        {
            var climbs = ukcReader.GetAllClimbs(userId);
            var mapMarkers = new List<MapMarkerDto>();

            foreach (var climb in climbs)
            {
                var crag = GetCrag(climb.UkcCragId);
                if (crag.Location == null)
                {
                    if (cragLocator.TryFindCrag(crag.CragName, out var location))
                    {
                        crag.Location = location;
                    }
                    else if (cragLocator.TryFindCrag(crag.County, crag.Country, out location))
                    {
                        crag.Location = location;
                    }
                }
                mapMarkers.Add(new MapMarkerDto()
                {
                    ClimbName = climb.ClimbName,
                    Grade = climb.Grade,
                    Location = crag?.Location
                });
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

        private UkcCrag GetCrag(int ukcCragId)
        {
            var crag = cragRepository.GetCrag(ukcCragId);
            if (crag == null)
            {
                return ukcReader.GetCragData(ukcCragId);
            }
            else
            {
                return new UkcCrag()
                {
                    CragName = crag.CragName,
                    UkcCragId = crag.UkcCragId,
                    Location = new MapLocation()
                    {
                        Latitude = crag.Latitude ?? 0,
                        Longitude = crag.Longitude ?? 0
                    }
                };
            }
        }
    }
}
