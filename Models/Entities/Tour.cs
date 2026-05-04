using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class Tour
    {
        public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Duration { get; set; } = string.Empty;
            public int Days { get; set; }
            public int Nights { get; set; }
            public double Rating { get; set; }
            public int TotalReviews { get; set; }
            public bool IsDeleted { get; set; } = false;
            public int CategoryId { get; set; }
            public Category? Category { get; set; }
            public int DestinationId { get; set; }
            public Destination? Destination { get; set; }
            public ICollection<TourImage> Images { get; set; } = new List<TourImage>();
            public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
            public ICollection<Review> Reviews { get; set; } = new List<Review>();
            public ICollection<Deal> Deals { get; set; } = new List<Deal>();
            public ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();
            public ICollection<TourSchedule> Schedules { get; set; } = new List<TourSchedule>();
            public ICollection<TourDestination> TourDestinations { get; set; } = new List<TourDestination>();
    }
}