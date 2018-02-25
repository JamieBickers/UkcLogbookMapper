using System.Collections.Generic;
using LocationMapper.Entities;

namespace LocationMapper.WebScrapers.Interfaces
{
    public interface IUkcReader
    {
        bool TryGetUserId(string userName, out int userId);
        IEnumerable<LogbookEntry> GetAllClimbs(int userId);
        (string County, string Country) GetRoughCragLocation(int cragId);
    }
}