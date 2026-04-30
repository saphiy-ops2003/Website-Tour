using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }
            public decimal Amount { get; set; }
            public DateTime PaymentDate { get; set; } = DateTime.Now;
            public string PaymentMethod { get; set; } = string.Empty;
            public string Status { get; set; } = "Completed";
            public int BookingId { get; set; }
            public Booking? Booking { get; set; }
    }
}