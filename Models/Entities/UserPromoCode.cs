using System;

namespace WebsiteTour.Models.Entities
{
    public class UserPromoCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        
        public int PromoCodeId { get; set; }
        public PromoCode? PromoCode { get; set; }
        
        public DateTime SavedAt { get; set; } = DateTime.Now;
        public bool IsUsed { get; set; } = false;
    }
}
