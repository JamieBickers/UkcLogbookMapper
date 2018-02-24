﻿using LocationMapper.Entities;
using LocationMapper.WebScrapers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LocationMapper.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LocationMapper.WebScrapers
{
    public class UkcReader : IUkcReader
    {
        private IUkcPageReader pageReader;
        private IUkcPageParser pageParser;

        public UkcReader() : this(new UkcPageReader(), new UkcPageParser())
        {

        }

        internal UkcReader(IUkcPageReader pageReader, IUkcPageParser pageParser)
        {
            this.pageReader = pageReader;
            this.pageParser = pageParser;
        }

        public IEnumerable<LogbookEntry> GetAllClimbs(string userName)
        {
            // Keep track of viewed pages to detect loops.
            var pages = new List<string>();

            if (!TryGetUserId(userName, out var userId))
            {
                return null;
            }

            IEnumerable<LogbookEntry> climbs = new List<LogbookEntry>();

            for (var i = 1; ; i++)
            {
                if (i > 100)
                {
                    throw new Exception("Too many pages");
                }

                var page = pageReader.GetUserLogbookPage(userId, i);
                if (pages.Contains(page))
                {
                    // In a loop
                    return climbs;
                }
                else if (pageParser.TryGetAllClimbsOnPage(page, out var climbsOnPage))
                {
                    climbs = climbs.Concat(climbsOnPage.ToList());
                    pages.Add(page);
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
                return (County: null, Country: null);
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
