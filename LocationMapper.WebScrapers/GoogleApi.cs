using LocationMapper.Entities;
using LocationMapper.WebScrapers.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LocationMapperTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LocationMapper.WebScrapers
{
    class GoogleApi : IGoogleApi
    {
        private const string GoogleApikey = "AIzaSyAbwGGr4DAAb-BwnLjrS3jEjRNepZSCLUM";

        public RootObject GoogleSearch(IEnumerable<string> queryParameters)
        {
            var searchableParameters = queryParameters.Select(SearchableString);

            var query = string.Join('+', searchableParameters);

            var request = WebRequest.Create(
                $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={query}&key={GoogleApikey}");

            request.Method = "GET";

            var response = request.GetResponse();

            var content = "";
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    content = streamReader.ReadToEnd();
                }
            }
            return JsonConvert.DeserializeObject<RootObject>(content);
        }

        private static string SearchableString(string search)
        {
            return search.Replace(' ', '+').Replace("&amp;", "and");
        }
    }
}
