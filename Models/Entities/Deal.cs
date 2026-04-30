using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class Deal
    {
        public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public double DiscountPercentage { get; set; }
            public DateTime ValidUntil { get; set; }
            public string? ImageUrl { get; set; }
            public int TourId { get; set; }
            public Tour? Tour { get; set; }
    }
}