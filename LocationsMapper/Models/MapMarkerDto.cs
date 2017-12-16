using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationsMapper.Models
{
    public class MapMarkerDto
    {
        public MapLocation Location { get; set; }
        public string ClimbName { get; set; }
        public string Grade { get; set; }
    }
}
