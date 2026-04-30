using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public ICollection<Tour> Tours { get; set; } = new List<Tour>();
    }
}