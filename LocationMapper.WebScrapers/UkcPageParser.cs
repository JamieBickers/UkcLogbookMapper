using AngleSharp.Parser.Html;
using LocationMapper.Entities;
using LocationMapper.WebScrapers.Interfaces;
using LocationMapper.WebScrapers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LocationMapper.WebScrapers.Entities;
using System;
using AngleSharp.Dom;

[assembly: InternalsVisibleTo("LocationMapper.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace LocationMapper.WebScrapers
{
    class UkcPageParser : IUkcPageParser
    {
        private HtmlParser parser;

        public UkcPageParser()
        {
            parser = new HtmlParser();
        }

        public bool TryGetAllClimbsOnPage(string page, out IEnumerable<LogbookEntry> climbs)
        {
            // observe from looking at the raw html that this returns what we want
            var parsedWebPage = parser.Parse(page);

            var rows = parsedWebPage
                .All
                .FirstOrDefault(element => element.LocalName == "tbody") // only one table in the page which is the one we want
                ?.Children
                .Where(node => node.LocalName == "tr"); // each row is one climb

            if ((rows?.Count() ?? 0) == 0)
            {
                climbs = null;
                return false;
            }

            climbs = rows
            .Select(row => row.Children.Where(element => element.LocalName == "td")) // get a list of the columns for each climb
            .Select(row => new
            {
                name = row.FirstOrDefault(column => column.ClassName == "climb")?.Children?.FirstOrDefault()?.InnerHtml,
                grade = row.FirstOrDefault(column => column.ClassName == "grade")?.InnerHtml,
                date = row.FirstOrDefault(column => column.ClassName == "logdate text-center")?.InnerHtml,
                cragLink = row.ToList().ElementAtOrDefault(6)?.InnerHtml
            })
            .Where(data => data.name != null && data.name != "")
            .Select(data => new LogbookEntry()
            {
                ClimbName = data.name,
                Grade = data.grade,
                Date = data.date.DeserialiseUkcFormattedDate(),
                UkcCragId = FindCragIdInLink(data.cragLink)
            });

            if (climbs.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetCragInformationFromCragPage(string page, out UkcCrag crag)
        {
            var parsedWebPage = parser.Parse(page);
            var cragName = GetCragnameFromParsedPage(parsedWebPage);

            var data = parsedWebPage
                ?.All
                ?.FirstOrDefault()
                ?.FirstElementChild
                ?.Children
                ?.FirstOrDefault(child => child.InnerHtml.Contains("\nvar id = "))
                ?.InnerHtml;

            if (string.IsNullOrWhiteSpace(data))
            {
                crag = null;
                return false;
            }

            var cragId = GetCragIdFromData(data);
            var latitudate = GetLatitudeFromData(data);
            var longitude = GetLongitudeFromData(data);

            var (County, Country) = TryGetRoughCragLocation(page);

            //These must not be null
            if (cragId == null || string.IsNullOrWhiteSpace(cragName))
            {
                crag = null;
                return false;
            }
            else
            {
                crag = new UkcCrag()
                {
                    CragName = cragName,
                    UkcCragId = cragId.Value,
                    Location = new MapLocation()
                    {
                        Latitude = latitudate,
                        Longitude = longitude
                    },
                    Country = Country,
                    County = County
                };
                return true;
            }
        }

        public bool TryGetUserIdOnSearchPage(string page, out string userIdAsString)
        {
            var hyperlinkContainingUserIdRegex = new Regex(@"profile.php\?id=[0-9]*");

            if (!hyperlinkContainingUserIdRegex.IsMatch(page ?? ""))
            {
                userIdAsString = null;
                return false;
            }
            else
            {
                var match = hyperlinkContainingUserIdRegex.Match(page);
                var url = match.ToString();
                var userIdRegex = new Regex("[0-9]+");
                var userIdMatch = userIdRegex.Match(url);

                userIdAsString = userIdMatch.ToString();
                return true;
            }
        }

        private (string County, string Country) TryGetRoughCragLocation(string page)
        {
            var parsedWebPage = parser.Parse(page);

            var locationDetailsList = parsedWebPage
                .All
                .FirstOrDefault(element => element.LocalName == "ol"); // only one ordered list on the page

            // Check really is a ukc crag page.
            var firstElementContent = locationDetailsList?.Children?.FirstOrDefault()?.OuterHtml;
            if (firstElementContent == "<li class=\"breadcrumb-item\"><a href=\"/logbook/\">Logbooks</a></li>")
            {
                var county = locationDetailsList
                    ?.Children[2]
                    ?.Children
                    ?.FirstOrDefault()
                    ?.InnerHtml;

                var country = locationDetailsList
                    ?.Children[1]
                    ?.Children
                    ?.FirstOrDefault()
                    ?.InnerHtml;

                return (County: county, Country: country);
            }
            else
            {
                return (County: null, Country: null);
            }
        }

        private int FindCragIdInLink(string cragLink)
        {
            var idSectionRegex = new Regex(@"id=[0-9]*");
            var cragId = idSectionRegex.Matches(cragLink).First().ToString();

            return int.Parse(cragId.Substring(3));
        }

        private int? GetCragIdFromData(string data)
        {
            var cragIdRegex = new Regex("var id = [0-9]+,");
            var debug = cragIdRegex.Match(data);
            var match = cragIdRegex.Match(data)?.Value;
            var idAsString = match?.Substring(9, match.Length - 10);
            if (int.TryParse(idAsString, out var id))
            {
                return id;
            }
            else
            {
                return null;
            }
        }

        private decimal GetLongitudeFromData(string data)
        {
            var latitudeRegex = new Regex(@"lng = -?[0-9]+\.[0-9]+,");
            var match = latitudeRegex.Match(data).Value;
            var latitudeAsString = match.Substring(6, match.Length - 7);
            return decimal.Parse(latitudeAsString);
        }

        private decimal GetLatitudeFromData(string data)
        {
            var longitudeRegex = new Regex(@"lat = -?[0-9]+\.[0-9]+,");
            var match = longitudeRegex.Match(data).Value;
            var longitudeAsString = match.Substring(6, match.Length - 7);
            return decimal.Parse(longitudeAsString);
        }

        private string GetCragnameFromParsedPage(AngleSharp.Dom.Html.IHtmlDocument parsedWebPage)
        {
            return parsedWebPage
                ?.All
                ?.FirstOrDefault()
                ?.FirstElementChild
                ?.FirstElementChild
                ?.InnerHtml
                ?.SubstringOrDefault("Ukc Logbook - ".Length);
        }
    }
}
