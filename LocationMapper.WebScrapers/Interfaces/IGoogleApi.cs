using LocationMapper.Entities;
using System.Collections.Generic;

namespace LocationMapper.WebScrapers.Interfaces
{
    interface IGoogleApi
    {
        RootObject GoogleSearch(IEnumerable<string> queryParameters);
    }
}
