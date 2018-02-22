using AngleSharp.Parser.Html;
using LocationMapper.Entities;
using LocationMapper.WebScrapers.Interfaces;
using LocationsMapper.WebScrapers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
                .Children
                .Where(node => node.LocalName == "tr"); // each row is one climb

            if (rows.Count() == 0)
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
                crag = row.ToList().GetIndexOrDefault(6)?.Children?.FirstOrDefault()?.InnerHtml, // the crag is the 7th column
                cragLink = row.ToList().GetIndexOrDefault(6)?.InnerHtml
            })
            .Where(data => data.name != null && data.name != "")
            .Select(data => new LogbookEntry()
            {
                ClimbName = data.name,
                Grade = data.grade,
                Date = UkcDateParser.CalculateDate(data.date),
                CragName = data.crag,
                CragId = FindCragIdInLink(data.cragLink)
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

        public bool TryGetRoughCragLocation(string page, out (string County, string Country) location)
        {
            var parsedWebPage = parser.Parse(page);

            var locationDetailsList = parsedWebPage
                .All
                .First(element => element.LocalName == "ol"); // only one ordered list on the page

            var county = locationDetailsList
                .Children[2]
                .Children
                .First()
                .InnerHtml;

            var country = locationDetailsList
                .Children[1]
                .Children
                .First()
                .InnerHtml;

            location = (County: county, Country: country);
            return true;
        }

        public bool TryGetUserIdOnPage(string page, out string userIdAsString)
        {
            var hyperlinkContainingUserIdRegex = new Regex(@"profile.php\?id=[0-9]*");

            if (!hyperlinkContainingUserIdRegex.IsMatch(page))
            {
                userIdAsString = "";
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

        private int FindCragIdInLink(string cragLink)
        {
            var idSectionRegex = new Regex(@"id=[0-9]*");
            var cragId = idSectionRegex.Matches(cragLink).First().ToString();

            return int.Parse(cragId.Substring(3));
        }
    }
}
