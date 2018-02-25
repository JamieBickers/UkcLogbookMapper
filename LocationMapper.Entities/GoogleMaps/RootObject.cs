using System.Collections.Generic;

namespace LocationMapper.Entities
{
    public class RootObject
    {
        public List<object> Html_Attributions { get; set; }
        public List<Result> Results { get; set; }
        public string Status { get; set; }
    }
}
