namespace LocationMapper.Entities
{
    public class Crag
    {
        public int id { get; set; }
        public int ukccragid { get; set; }
        public string cragname { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public bool exactlocation { get; set; }
    }
}
