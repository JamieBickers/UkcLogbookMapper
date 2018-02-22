using LocationMapper.Entities;
using LocationMapper.WebScrapers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LocationMapperTests")]

namespace LocationMapper.WebScrapers
{
    public class UkcReader : IUkcReader
    {
        private UkcPageReader pageReader;
        private UkcPageParser pageParser;

        public UkcReader()
        {
            pageReader = new UkcPageReader();
            pageParser = new UkcPageParser();
        }

        public IEnumerable<LogbookEntry> GetAllClimbs(string userName)
        {
            if (!TryGetUserId(userName, out var userId))
            {
                return new List<LogbookEntry>();
            }

            var climbs = new List<LogbookEntry>();

            for (var i = 1; ; i++)
            {
                if (i > 100)
                {
                    throw new Exception("Too many pages");
                }

                var page = pageReader.GetUserLogbookPage(userId, i);
                if (pageParser.TryGetAllClimbsOnPage(page, out var climbsOnPage))
                {
                    climbs.Concat(climbsOnPage);
                }
                else
                {
                    return climbs;
                }
            }
        }

        public (string County, string Country) GetRoughCragLocation(int cragId)
        {
            var page = pageReader.GetCragPage(cragId);
            if (pageParser.TryGetRoughCragLocation(page, out var location))
            {
                return location;
            }
            else
            {
                return (County: "", Country: "");
            }
        }

        private bool TryGetUserId(string userName, out int userId)
        {
            var page = pageReader.GetSearchPage(userName);
            
            if (pageParser.TryGetUserIdOnSearchPage(page, out var userIdAsString))
            {
                return int.TryParse(userIdAsString, out userId);
            }
            else
            {
                userId = 0;
                return false;
            }
        }
    }
}
