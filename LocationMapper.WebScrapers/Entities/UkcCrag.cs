using LocationMapper.Entities;

namespace LocationMapper.WebScrapers.Entities
{
    public class UkcCrag
    {
        public int UkcCragId { get; set; }
        public string CragName { get; set; }
        public MapLocation Location { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
    }
}
