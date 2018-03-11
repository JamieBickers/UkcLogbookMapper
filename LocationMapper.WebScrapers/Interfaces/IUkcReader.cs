using System.Collections.Generic;
using LocationMapper.Entities;
using LocationMapper.WebScrapers.Entities;

namespace LocationMapper.WebScrapers.Interfaces
{
    public interface IUkcReader
    {
        bool TryGetUserId(string userName, out int userId);
        IEnumerable<LogbookEntry> GetAllClimbs(int userId);
        UkcCrag GetCragData(int ukcCragId);
    }
}