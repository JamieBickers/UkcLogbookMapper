using System;

namespace LocationMapper.Entities
{
    public class LogbookEntry
    {
        public string ClimbName { get; set; }
        public string Grade { get; set; }
        public DateTimeOffset Date { get; set; }
        public int UkcCragId { get; set; }
    }
}
