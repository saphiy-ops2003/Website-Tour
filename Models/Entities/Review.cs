using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; } = string.Empty;
            public DateTime ReviewDate { get; set; } = DateTime.Now;
            public int UserId { get; set; }
            public User? User { get; set; }
            public int TourId { get; set; }
            public Tour? Tour { get; set; }
    }
}