using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int RegionId { get; set; }
        public Region? Region { get; set; }
        public ICollection<Tour> Tours { get; set; } = new List<Tour>();
        public ICollection<TourDestination> TourDestinations { get; set; } = new List<TourDestination>();
    }
}