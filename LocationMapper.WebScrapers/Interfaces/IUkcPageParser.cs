using System.Collections.Generic;
using LocationMapper.Entities;

namespace LocationMapper.WebScrapers.Interfaces
{
    interface IUkcPageParser
    {
        bool TryGetAllClimbsOnPage(string page, out IEnumerable<LogbookEntry> climbs);
        bool TryGetRoughCragLocation(string page, out (string County, string Country) location);
        bool TryGetUserIdOnSearchPage(string page, out string userIdAsString);
    }
}