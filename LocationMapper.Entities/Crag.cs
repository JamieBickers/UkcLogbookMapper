namespace LocationMapper.Entities
{
    public class Crag
    {
        public int ID { get; set; }
        public int UkcCragId { get; set; }
        public string CragName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool ExactLocation { get; set; }
    }
}
