using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationsMapper.Models
{
    public class LogbookEntry
    {
        public LogbookEntry() { }

        public LogbookEntry(string climbName, string grade, DateTimeOffset date, string cragName, int cragId)
        {
            ClimbName = climbName;
            Grade = grade;
            Date = date;
            CragName = cragName;
            CragId = cragId;
        }

        public string ClimbName { get; set; }
        public string Grade { get; set; }
        public DateTimeOffset Date { get; set; }
        public string CragName { get; set; }
        public int CragId { get; set; }
    }
}
