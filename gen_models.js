const fs = require('fs');
const path = require('path');

const modelsDir = path.join(__dirname, 'Models', 'Entities');
if (!fs.existsSync(modelsDir)) {
    fs.mkdirSync(modelsDir, { recursive: true });
}

const entities = [
    { name: "Role", props: "public int Id { get; set; }\n    public string Name { get; set; } = string.Empty;\n    public ICollection<User> Users { get; set; } = new List<User>();" },
    { name: "User", props: "public int Id { get; set; }\n    public string Username { get; set; } = string.Empty;\n    public string Email { get; set; } = string.Empty;\n    public string PasswordHash { get; set; } = string.Empty;\n    public int RoleId { get; set; }\n    public Role? Role { get; set; }\n    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();\n    public ICollection<Review> Reviews { get; set; } = new List<Review>();" },
    { name: "Category", props: "public int Id { get; set; }\n    public string Name { get; set; } = string.Empty;\n    public ICollection<Tour> Tours { get; set; } = new List<Tour>();" },
    { name: "Destination", props: "public int Id { get; set; }\n    public string Name { get; set; } = string.Empty;\n    public string Region { get; set; } = string.Empty;\n    public ICollection<Tour> Tours { get; set; } = new List<Tour>();" },
    { name: "Tour", props: "public int Id { get; set; }\n    public string Title { get; set; } = string.Empty;\n    public string Description { get; set; } = string.Empty;\n    public decimal Price { get; set; }\n    public string Duration { get; set; } = string.Empty;\n    public string? Badge { get; set; }\n    public double Rating { get; set; }\n    public int CategoryId { get; set; }\n    public Category? Category { get; set; }\n    public int DestinationId { get; set; }\n    public Destination? Destination { get; set; }\n    public ICollection<TourImage> Images { get; set; } = new List<TourImage>();\n    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();\n    public ICollection<Review> Reviews { get; set; } = new List<Review>();\n    public ICollection<Deal> Deals { get; set; } = new List<Deal>();" },
    { name: "TourImage", props: "public int Id { get; set; }\n    public string ImageUrl { get; set; } = string.Empty;\n    public bool IsPrimary { get; set; }\n    public int TourId { get; set; }\n    public Tour? Tour { get; set; }" },
    { name: "Booking", props: "public int Id { get; set; }\n    public DateTime BookingDate { get; set; } = DateTime.Now;\n    public decimal TotalPrice { get; set; }\n    public string Status { get; set; } = \"Pending\";\n    public int UserId { get; set; }\n    public User? User { get; set; }\n    public int TourId { get; set; }\n    public Tour? Tour { get; set; }\n    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();\n    public Payment? Payment { get; set; }" },
    { name: "BookingDetail", props: "public int Id { get; set; }\n    public string PassengerName { get; set; } = string.Empty;\n    public int PassengerAge { get; set; }\n    public int BookingId { get; set; }\n    public Booking? Booking { get; set; }" },
    { name: "Payment", props: "public int Id { get; set; }\n    public decimal Amount { get; set; }\n    public DateTime PaymentDate { get; set; } = DateTime.Now;\n    public string PaymentMethod { get; set; } = string.Empty;\n    public string Status { get; set; } = \"Completed\";\n    public int BookingId { get; set; }\n    public Booking? Booking { get; set; }" },
    { name: "Review", props: "public int Id { get; set; }\n    public int Rating { get; set; }\n    public string Comment { get; set; } = string.Empty;\n    public DateTime ReviewDate { get; set; } = DateTime.Now;\n    public int UserId { get; set; }\n    public User? User { get; set; }\n    public int TourId { get; set; }\n    public Tour? Tour { get; set; }" },
    { name: "Deal", props: "public int Id { get; set; }\n    public string Title { get; set; } = string.Empty;\n    public double DiscountPercentage { get; set; }\n    public DateTime ValidUntil { get; set; }\n    public string? ImageUrl { get; set; }\n    public int TourId { get; set; }\n    public Tour? Tour { get; set; }" }
];

entities.forEach(entity => {
    const content = `using System;
using System.Collections.Generic;

namespace WebsiteTour.Models.Entities
{
    public class ${entity.name}
    {
        ${entity.props.replace(/\n/g, "\n        ")}
    }
}`;
    fs.writeFileSync(path.join(modelsDir, `${entity.name}.cs`), content);
});

const dbContextContent = `using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models.Entities;

namespace WebsiteTour.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourImage> TourImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Deal> Deals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Decimal precision configs
            modelBuilder.Entity<Tour>().Property(t => t.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Booking>().Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");

            // Relationships
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}`;

fs.writeFileSync(path.join(__dirname, 'Models', 'AppDbContext.cs'), dbContextContent);

console.log("Models generated successfully.");
