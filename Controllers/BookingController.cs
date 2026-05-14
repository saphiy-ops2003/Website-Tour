using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;

public class BookingController : Controller
{
    private readonly AppDbContext _context;

    public BookingController(AppDbContext context)
    {
        _context = context;
    }

    // ================== CHECKOUT GET ==================
    [Authorize]
    public async Task<IActionResult> Checkout(int tourId, int adults = 2, int children = 0, string promoCode = "", int scheduleId = 0)
    {
        var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == tourId);
        if (tour == null) return RedirectToAction("Tours", "Tour");

        var schedule = await _context.TourSchedules
            .FirstOrDefaultAsync(s => s.Id == scheduleId && s.TourId == tourId);

        if (schedule == null)
            return RedirectToAction("TourDetail", "Tour", new { id = tourId });

        int totalGuests = adults + children;
        if (schedule.AvailableSeats < totalGuests)
        {
            TempData["ErrorMessage"] = $"Chỉ còn {schedule.AvailableSeats} chỗ.";
            return RedirectToAction("TourDetail", "Tour", new { id = tourId });
        }

        decimal discount = 0;
        string validPromoCode = "";

        if (!string.IsNullOrEmpty(promoCode))
        {
            decimal subTotal = (tour.Price * adults) + (tour.Price * 0.7m * children);

            var promo = await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == promoCode.ToUpper() && p.IsActive && p.ValidUntil >= DateTime.Now);

            if (promo != null && subTotal >= promo.MinOrderValue)
            {
                discount = promo.DiscountAmount;
                validPromoCode = promo.Code;
            }
        }

        ViewBag.Adults = adults;
        ViewBag.Children = children;
        ViewBag.PromoCode = validPromoCode;
        ViewBag.Discount = discount;
        ViewBag.Schedule = schedule;

        return View(tour);
    }

    // ================== CHECKOUT POST ==================
    [HttpPost]
    [Authorize]
    [ActionName("Checkout")]
    public async Task<IActionResult> CheckoutPost(int tourId, int adults, int children, string promoCode, int scheduleId)
    {
        var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == tourId);
        if (tour == null) return RedirectToAction("Tours", "Tour");

        var schedule = await _context.TourSchedules
            .FirstOrDefaultAsync(s => s.Id == scheduleId && s.TourId == tourId);

        if (schedule == null)
            return RedirectToAction("TourDetail", "Tour", new { id = tourId });

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        decimal adultPrice = tour.Price;
        decimal childPrice = tour.Price * 0.7m;
        decimal subTotal = (adultPrice * adults) + (childPrice * children);

        int totalGuests = adults + children;
        if (schedule.AvailableSeats < totalGuests)
        {
            TempData["ErrorMessage"] = "Không đủ chỗ.";
            return RedirectToAction("TourDetail", "Tour", new { id = tourId });
        }

        decimal discount = 0;

        if (!string.IsNullOrEmpty(promoCode))
        {
            var promo = await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == promoCode.ToUpper() && p.IsActive && p.ValidUntil >= DateTime.Now);

            if (promo != null && subTotal >= promo.MinOrderValue)
            {
                discount = promo.DiscountAmount;
            }
            else
            {
                promoCode = "";
            }
        }

        schedule.AvailableSeats -= totalGuests;

        var booking = new Booking
        {
            TourId = tour.Id,
            TourScheduleId = scheduleId,
            UserId = userId,
            BookingDate = DateTime.Now,
            AdultCount = adults,
            ChildCount = children,
            DiscountAmount = discount,
            PromoCode = promoCode,
            TotalPrice = subTotal - discount + 10,
            Status = "Pending"
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        await TrackUserBehaviorAsync(userId, tour.Id, "booking", 5.0);

        TempData["SuccessMessage"] = "Đặt Tour thành công!";
        return RedirectToAction("Bookings");
    }

    // ================== DANH SÁCH ĐƠN ==================
    [Authorize]
    public async Task<IActionResult> Bookings()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        var bookings = await _context.Bookings
            .Include(b => b.Tour)
            .ThenInclude(t => t.Images)
            .Include(b => b.Tour.Destination)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

        return View(bookings);
    }

    // ================== XEM VÉ ==================
    [Authorize]
    public async Task<IActionResult> Ticket(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        var booking = await _context.Bookings
            .Include(b => b.Tour)
            .Include(b => b.Tour.Destination)
            .Include(b => b.TourSchedule)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

        if (booking == null) return NotFound();

        return View(booking);
    }

    // ================== TRACK ==================
    private async Task TrackUserBehaviorAsync(int userId, int tourId, string behaviorType, double weight)
    {
        _context.UserTourBehaviors.Add(new UserTourBehavior
        {
            UserId = userId,
            TourId = tourId,
            BehaviorType = behaviorType,
            Weight = weight,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }
}