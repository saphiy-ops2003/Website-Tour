using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class TourImage
    {
        public int Id { get; set; }
            public string ImageUrl { get; set; } = string.Empty;
            public bool IsPrimary { get; set; }
            public int TourId { get; set; }
            public Tour? Tour { get; set; }
    }
}