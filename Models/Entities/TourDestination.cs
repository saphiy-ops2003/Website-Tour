namespace WebsiteTour.Models.Entities
{
    public class TourDestination
    {
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
        public int DestinationId { get; set; }
        public Destination? Destination { get; set; }
    }
}
