using LocationMapper.Entities;
using LocationMapper.Interfaces.WebScrapers;
using LocationMapper.WebScrapers.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LocationMapperTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LocationMapper.WebScrapers
{
    public class CragLocator : ICragLocator
    {
        private IGoogleApi googleApi;

        public CragLocator() : this(new GoogleApi())
        {

        }

        internal CragLocator(IGoogleApi googleApi)
        {
            this.googleApi = googleApi;
        }

        public bool TryFindCrag(string cragName, out MapLocation location)
        {
            return TrySearchGoogleMaps(new List<string>() { cragName }, out location);
        }

        public bool TryFindCrag(string countyName, string countryName, out MapLocation location)
        {
            return TrySearchGoogleMaps(new List<string>() { countyName, countryName }, out location);
        }

        private bool TrySearchGoogleMaps(IEnumerable<string> queryParameters, out MapLocation location)
        {
            var searchResult = googleApi.GoogleSearch(queryParameters);

            var resultLocation = searchResult?.Results?.ElementAtOrDefault(0)?.Geometry?.Location;

            if (resultLocation?.Lat == null || resultLocation?.Lng == null)
            {
                location = null;
                return false;
            }
            else
            {
                location = new MapLocation()
                {
                    Latitude = (decimal)resultLocation.Lat,
                    Longitude = (decimal)resultLocation.Lng
                };
                return true;
            }
        }
    }
}
