using LocationsMapper.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace LocationsMapper.Models
{
    public static class CragLocator
    {
        private const string GoogleApikey = "AIzaSyAbwGGr4DAAb-BwnLjrS3jEjRNepZSCLUM";

        public static MapLocation FindCrag(string cragName, int ukcCragId, CragLocationContext context)
        {
            var queries = new DatabaseQueries(context);

            if (queries.TryGetLocationFromDatabase(ukcCragId, out var location))
            {
                return location;
            }
            try
            {
                return SearchGoogleMaps(ukcCragId, cragName, context);
            }
            catch
            {
                return FindCountyLocation(ukcCragId, cragName, context);
            }
        }

        private static MapLocation FindCountyLocation(int ukcCragId, string cragName, CragLocationContext context)
        {
            var countyName = WebScraper.GetCountyName(ukcCragId, out var countryName);
            var queries = new DatabaseQueries(context);

            if (countyName == "Indoors")
            {
                return null;
            }

            var searchableCountyName = SearchableString(countyName);
            var searchableCountryName = SearchableString(countryName);

            var request = WebRequest.Create(
                $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={searchableCountyName}+{searchableCountryName}&key={GoogleApikey}");

            request.Method = "GET";

            var response = request.GetResponse();

            var content = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    content = streamReader.ReadToEnd();
                }
            }

            var result = JsonConvert.DeserializeObject<RootObject>(content);

            var location = result.results[0].geometry.location;

            queries.AddCragToDatabase(ukcCragId, cragName, (decimal)location.lat, (decimal)location.lng, false);

            return new MapLocation((decimal)location.lat, (decimal)location.lng);
        }

        private static MapLocation SearchGoogleMaps(int cragId, string cragName, CragLocationContext context)
        {
            var queries = new DatabaseQueries(context);
            var searchableCragName = SearchableString(cragName);

            var request = WebRequest.Create(
                $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={searchableCragName}&key={GoogleApikey}");

            request.Method = "GET";

            var response = request.GetResponse();

            var content = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    content = streamReader.ReadToEnd();
                }
            }

            var result = JsonConvert.DeserializeObject<RootObject>(content);

            var location = result.results[0].geometry.location;

            queries.AddCragToDatabase(cragId, cragName, (decimal)location.lat, (decimal)location.lng, true);

            return new MapLocation((decimal)location.lat, (decimal)location.lng);
        }

        private static string SearchableString(string search)
        {
            return search.Replace(' ', '+').Replace("&amp;", "and");
        }
    }
}
