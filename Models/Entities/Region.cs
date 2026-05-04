namespace WebsiteTour.Models.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }
}
