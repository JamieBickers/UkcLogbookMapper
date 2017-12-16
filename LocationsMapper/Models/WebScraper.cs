using System.Collections.Generic;
using System.Net;
using AngleSharp.Parser.Html;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace LocationsMapper.Models
{
    public static class WebScraper
    {
        public static IEnumerable<LogbookEntry> GetAllClimbs(int ukcUserId)
        {
            var previousPage = "";
            var climbs = new List<LogbookEntry>();

            for (var i = 0; ; i++)
            {
                try
                {
                    var page = new WebClient().DownloadString(
                    $"https://www.ukclimbing.com/logbook/showlog.html?id={ukcUserId}&nresults=100&pg={i + 1}#my_logbook");


                    // check if page has no climbs
                    var emptyClimbsRegex = new Regex("This user hasn't setup a Logbook of their climbs yet.");
                    if (emptyClimbsRegex.IsMatch(page))
                    {
                        break;
                    }

                    // check if in a loop
                    if (page == previousPage)
                    {
                        break;
                    }
                    else
                    {
                        climbs.AddRange(GetAllClimbsOnThePage(page));
                    }

                    if (i > 100)
                    {
                        throw new Exception("Too many pages.");
                    }

                    previousPage = page;
                }
                catch
                {
                    break;
                }
            }
            return climbs;
        }

        public static string GetCountyName(int cragId, out string country)
        {
            var page = new WebClient().DownloadString(
                $"https://www.ukclimbing.com/logbook/crag.php?id={cragId}");

            var parser = new HtmlParser();
            var parsedWebPage = parser.Parse(page);

            var locationDetailsList = parsedWebPage
                .All
                .First(element => element.LocalName == "ol"); // only one ordered list on the page

            var county = locationDetailsList
                .Children[2]
                .Children
                .First()
                .InnerHtml;

            country = locationDetailsList
                .Children[1]
                .Children
                .First()
                .InnerHtml;

            return county;
        }

        // returns -1 if not found
        public static int GetUserId(string userName)
        {
            var page = new WebClient().DownloadString(
                $"https://www.ukclimbing.com/user/profiles.php?keyword={userName}&wide=0");

            var hyperlinkContainingUserIdRegex = new Regex(@"profile.php\?id=[0-9]*");

            if (!hyperlinkContainingUserIdRegex.IsMatch(page))
            {
                return -1;
            }

            var match = hyperlinkContainingUserIdRegex.Match(page);

            var url = match.ToString();

            var userIdRegex = new Regex("[0-9]+");

            var userIdMatch = userIdRegex.Match(url);

            var userId = userIdMatch.ToString();

            return int.Parse(userId);
        }

        private static IEnumerable<LogbookEntry> GetAllClimbsOnThePage(string page)
        {
            // observe from looking at the raw html that this returns what we want
            var parser = new HtmlParser();
            var parsedWebPage = parser.Parse(page);

            var climbs = parsedWebPage
                .All
                .First(element => element.LocalName == "tbody") // only one table in the page which is the one we want
                .Children
                .Where(node => node.LocalName == "tr") // each row is one climb
                .Select(row => row.Children.Where(element => element.LocalName == "td")) // get a list of the columns for each climb
                .Select(row => new
                {
                    name = row.First(column => column.ClassName == "climb").Children.First().InnerHtml,
                    grade = row.First(column => column.ClassName == "grade").InnerHtml,
                    date = row.First(column => column.ClassName == "logdate text-center").InnerHtml,
                    crag = row.ToList()[6].Children.First().InnerHtml, // the crag is the 7th column
                    cragLink = row.ToList()[6].InnerHtml
                })
                .Select(data => new LogbookEntry()
                {
                    ClimbName = data.name,
                    Grade = data.grade,
                    Date = UkcDateParser.CalculateDate(data.date),
                    CragName = data.crag,
                    CragId = FindCragIdInLink(data.cragLink)
                });

            return climbs;
        }

        private static int FindCragIdInLink(string cragLink)
        {
            var idSectionRegex = new Regex(@"id=[0-9]*");
            var cragId = idSectionRegex.Matches(cragLink).First().ToString();

            return int.Parse(cragId.Substring(3));
        }
    }
}
