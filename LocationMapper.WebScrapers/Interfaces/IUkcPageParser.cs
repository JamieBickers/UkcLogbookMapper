using System.Collections.Generic;
using LocationMapper.Entities;

namespace LocationMapper.WebScrapers.Interfaces
{
    interface IUkcPageParser
    {
        bool TryGetAllClimbsOnPage(string page, out IEnumerable<LogbookEntry> climbs);
        bool TryGetRoughCragLocation(string page, out (string County, string Country) location);
        bool TryGetUserIdOnPage(string page, out string userIdAsString);
    }
}