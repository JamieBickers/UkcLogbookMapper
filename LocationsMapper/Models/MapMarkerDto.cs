using LocationMapper.Entities;

namespace LocationMapper.Models
{
    public class MapMarkerDto
    {
        public MapLocation Location { get; set; }
        public string ClimbName { get; set; }
        public string Grade { get; set; }
    }
}
