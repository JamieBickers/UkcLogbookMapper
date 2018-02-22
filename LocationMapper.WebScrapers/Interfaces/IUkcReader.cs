using System.Collections.Generic;
using LocationMapper.Entities;

namespace LocationMapper.WebScrapers.Interfaces
{
    public interface IUkcReader
    {
        IEnumerable<LogbookEntry> GetAllClimbs(string userName);
        (string County, string Country) GetRoughCragLocation(int cragId);
    }
}