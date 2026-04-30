using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class BookingDetail
    {
        public int Id { get; set; }
            public string PassengerName { get; set; } = string.Empty;
            public int PassengerAge { get; set; }
            public int BookingId { get; set; }
            public Booking? Booking { get; set; }
    }
}