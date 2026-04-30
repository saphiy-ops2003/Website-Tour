using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteTour.Models.Entities
{
    public class PromoCode
    {
        public int Id { get; set; }
        
        [Required]
        public string Code { get; set; } = string.Empty;
        
        public decimal DiscountAmount { get; set; }
        
        public decimal MinOrderValue { get; set; } // Condition: Min order total to apply
        
        public string Description { get; set; } = string.Empty; // e.g., "Giảm 50$ cho đơn từ 200$"
        public string Title { get; set; } = string.Empty; // e.g., "Mã Giảm Giá Mùa Hè"
        public string Category { get; set; } = "Ưu đãi"; // For display tag like "Du Lịch Mùa Hè"
        
        public DateTime ValidUntil { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public ICollection<UserPromoCode> UserPromoCodes { get; set; } = new List<UserPromoCode>();
    }
}
