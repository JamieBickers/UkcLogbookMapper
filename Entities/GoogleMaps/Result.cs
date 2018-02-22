using System.Collections.Generic;

namespace LocationMapper.Entities
{
    public class Result
    {
        public string Formatted_Address { get; set; }
        public Geometry Geometry { get; set; }
        public string Ccon { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Photo> Photos { get; set; }
        public string Place_Id { get; set; }
        public double Rating { get; set; }
        public string Reference { get; set; }
        public List<string> Types { get; set; }
    }
}
