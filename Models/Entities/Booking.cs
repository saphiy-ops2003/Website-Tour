using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class Booking
    {
        public int Id { get; set; }
            public DateTime BookingDate { get; set; } = DateTime.Now;
            public decimal TotalPrice { get; set; }
            public string Status { get; set; } = "Pending";
            
            public int AdultCount { get; set; } = 1;
            public int ChildCount { get; set; } = 0;
            public decimal DiscountAmount { get; set; } = 0;
            public string? PromoCode { get; set; }
            public int UserId { get; set; }
            public User? User { get; set; }
            public int TourId { get; set; }
            public Tour? Tour { get; set; }
            public int? TourScheduleId { get; set; }
            public TourSchedule? TourSchedule { get; set; }
            public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
            public Payment? Payment { get; set; }
    }
}