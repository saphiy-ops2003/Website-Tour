namespace WebsiteTour.Models.Entities
{
    public class UserTourBehavior
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
        public string BehaviorType { get; set; } = string.Empty; // view, booking
        public double Weight { get; set; } = 1.0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
