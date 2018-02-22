using LocationMapper.WebScrapers.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LocationMapper.WebScrapers
{
    class UkcPageReader : IUkcPageReader
    {
        private WebClient webClient;

        public UkcPageReader()
        {
            webClient = new WebClient();
        }

        public string GetCragPage(int cragId)
        {
            var url = $"https://www.ukclimbing.com/logbook/crag.php?id={cragId}";
            return webClient.DownloadString(url);
        }

        public string GetUserLogbookPage(int userId, int pageNumber)
        {
            var url = $"https://www.ukclimbing.com/logbook/showlog.html?id={userId}&nresults=100&pg={pageNumber}#my_logbook";
            return webClient.DownloadString(url);
        }

        public string GetSearchPage(string searchTerm)
        {
            var url = $"https://www.ukclimbing.com/user/profiles.php?keyword={searchTerm}&wide=0";
            return webClient.DownloadString(url);
        }
    }
}
