using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public int RoleId { get; set; }
            public Role? Role { get; set; }
            public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
            public ICollection<Review> Reviews { get; set; } = new List<Review>();
            public ICollection<UserPromoCode> SavedPromoCodes { get; set; } = new List<UserPromoCode>();
    }
}