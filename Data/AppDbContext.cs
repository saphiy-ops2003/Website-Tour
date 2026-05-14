using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models.Entities;

namespace WebsiteTour.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TourDestination> TourDestinations { get; set; }
        public DbSet<TourImage> TourImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<TourSchedule> TourSchedules { get; set; }
        public DbSet<UserPromoCode> UserPromoCodes { get; set; }
        public DbSet<UserTourBehavior> UserTourBehaviors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Decimal precision configs
            modelBuilder.Entity<Tour>().Property(t => t.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Booking>().Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Booking>().Property(b => b.DiscountAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PromoCode>().Property(p => p.DiscountAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PromoCode>().Property(p => p.MinOrderValue).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<TourSchedule>().Property(s => s.Price).HasColumnType("decimal(18,2)");

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

            modelBuilder.Entity<UserTourBehavior>()
                .HasOne(ub => ub.User)
                .WithMany()
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTourBehavior>()
                .HasOne(ub => ub.Tour)
                .WithMany()
                .HasForeignKey(ub => ub.TourId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTourBehavior>()
                .HasIndex(ub => new { ub.UserId, ub.CreatedAt });

            modelBuilder.Entity<Region>()
                .HasIndex(r => r.Slug)
                .IsUnique();

            modelBuilder.Entity<Destination>()
                .HasIndex(d => d.Slug)
                .IsUnique();

            modelBuilder.Entity<Destination>()
                .HasOne(d => d.Region)
                .WithMany(r => r.Destinations)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TourDestination>()
                .HasKey(td => new { td.TourId, td.DestinationId });

            modelBuilder.Entity<TourDestination>()
                .HasOne(td => td.Tour)
                .WithMany(t => t.TourDestinations)
                .HasForeignKey(td => td.TourId);

            modelBuilder.Entity<TourDestination>()
                .HasOne(td => td.Destination)
                .WithMany(d => d.TourDestinations)
                .HasForeignKey(td => td.DestinationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}