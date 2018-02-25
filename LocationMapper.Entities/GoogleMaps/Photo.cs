using System.Collections.Generic;

namespace LocationMapper.Entities
{
    public class Photo
    {
        public int Height { get; set; }
        public List<string> Html_Attributions { get; set; }
        public string Photo_Reference { get; set; }
        public int Width { get; set; }
    }
}
