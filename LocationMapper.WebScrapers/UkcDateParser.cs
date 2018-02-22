using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("LocationMapperTests")]

namespace LocationsMapper.WebScrapers
{
    static class UkcDateParser
    {
        private static Dictionary<string, int> Months = new Dictionary<string, int>()
            {
                { "Jan", 1 },
                {"Feb", 2 },
                { "Mar", 3 },
                { "Apr", 4 },
                { "May", 5 },
                { "Jun", 6 },
                { "Jul", 7 },
                { "Aug", 8 },
                { "Sep", 9 },
                { "Oct", 10 },
                { "Nov", 11 },
                { "Dec", 12 }
            };

        public static DateTimeOffset CalculateDate(string date)
        {
            // means current year but unknown month
            if (date == "??")
            {
                return new DateTimeOffset(DateTimeOffset.Now.Year, 6, 15, 0, 0, 0, new TimeSpan(0));
            }

            // unknown month will look like ??/15
            var unknownMonthRegex = new Regex(@"\?\?\/[0-9]");
            var monthIsUnknown = unknownMonthRegex.IsMatch(date);

            // unknown day will look like ?/Apr (for current year) or ?/Apr/15
            var unknownDayRegex = new Regex(@"\?\/[a-zA-Z]");
            var dayIsUnknown = unknownDayRegex.IsMatch(date);

            if (monthIsUnknown)
            {
                var year = date.Where(it => int.TryParse(it.ToString(), out var result));
                return new DateTimeOffset(int.Parse(string.Concat(year)), 6, 15, 0, 0, 0, new TimeSpan(0)); // mid year
            }
            else if (dayIsUnknown)
            {
                if (char.IsDigit(date.Last())) // means not current year
                {
                    var year = int.Parse(date.Split('/').Last());
                    return new DateTimeOffset(year, Months[date.Substring(2, 3)], 15, 0, 0, 0, new TimeSpan(0));
                }
                else
                {
                    return new DateTimeOffset(DateTimeOffset.Now.Year, Months[date.Substring(2)], 15, 0, 0, 0, new TimeSpan(0));
                }
            }
            else
            {
                return DateTimeOffset.Parse(date);
            }
        }
    }
}
