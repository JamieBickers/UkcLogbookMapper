using LocationMapper.WebScrapers.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("LocationMapper.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LocationMapper.WebScrapers
{
    class UkcPageReader : IUkcPageReader
    {
        //Should only have one HttpClient per application. Refactor this if I use one outside this class.
        private static HttpClient httpClient;

        public UkcPageReader()
        {
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }
        }

        public string GetCragPage(int cragId)
        {
            var url = $"https://www.ukclimbing.com/logbook/crag.php?id={cragId}";
            return httpClient.GetStringAsync(url).Result;
        }

        public string GetUserLogbookPage(int userId, int pageNumber)
        {
            var url = $"https://www.ukclimbing.com/logbook/showlog.html?id={userId}&nresults=100&pg={pageNumber}#my_logbook";
            return httpClient.GetStringAsync(url).Result;
        }

        public string GetSearchPage(string searchTerm)
        {
            var url = $"https://www.ukclimbing.com/user/profiles.php?keyword={searchTerm}&wide=0";
            return httpClient.GetStringAsync(url).Result;
        }
    }
}
