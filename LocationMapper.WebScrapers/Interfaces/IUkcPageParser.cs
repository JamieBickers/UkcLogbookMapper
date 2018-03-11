using System.Collections.Generic;
using LocationMapper.Entities;
using LocationMapper.WebScrapers.Entities;

namespace LocationMapper.WebScrapers.Interfaces
{
    interface IUkcPageParser
    {
        bool TryGetAllClimbsOnPage(string page, out IEnumerable<LogbookEntry> climbs);
        bool TryGetUserIdOnSearchPage(string page, out string userIdAsString);
        bool TryGetCragInformationFromCragPage(string page, out UkcCrag crag);
    }
}