using System;

namespace WebsiteTour.Models.Entities
{
    public class TourSchedule
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour? Tour { get; set; }
        
        public DateTime StartDate { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal Price { get; set; }
    }
}
