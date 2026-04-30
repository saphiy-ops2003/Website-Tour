using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;

namespace WebsiteTour.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // ===== DASHBOARD =====
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalTours = await _context.Tours.CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();
            ViewBag.TotalRevenue = await _context.Bookings.Where(b => b.Status == "Confirmed").SumAsync(b => b.TotalPrice);
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            
            var recentBookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Tour)
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .ToListAsync();

            return View(recentBookings);
        }
        
        // ===== BOOKINGS =====
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Tour)
                .Include(b => b.TourSchedule)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
            return View(bookings);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateBookingStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật trạng thái thành công!";
            }
            return RedirectToAction("Bookings");
        }

        // ===== TOURS =====
        public async Task<IActionResult> Tours()
        {
            var tours = await _context.Tours
                .Include(t => t.Destination)
                .Include(t => t.Category)
                .Include(t => t.Schedules)
                .Include(t => t.Bookings)
                .ToListAsync();
            return View(tours);
        }

        public async Task<IActionResult> CreateTour()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Destinations = await _context.Destinations.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(Tour model, string imageUrl)
        {
            if (!string.IsNullOrEmpty(model.Title))
            {
                _context.Tours.Add(model);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    _context.TourImages.Add(new TourImage { TourId = model.Id, ImageUrl = imageUrl });
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = $"Đã tạo Tour \"{model.Title}\" thành công!";
                return RedirectToAction("Tours");
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Destinations = await _context.Destinations.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> EditTour(int id)
        {
            var tour = await _context.Tours
                .Include(t => t.Images)
                .Include(t => t.Schedules.Where(s => s.StartDate > DateTime.Now).OrderBy(s => s.StartDate))
                .FirstOrDefaultAsync(t => t.Id == id);
            if (tour == null) return RedirectToAction("Tours");

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Destinations = await _context.Destinations.ToListAsync();
            return View(tour);
        }

        [HttpPost]
        public async Task<IActionResult> EditTour(int id, Tour model, string imageUrl)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                tour.Title = model.Title;
                tour.Description = model.Description;
                tour.Price = model.Price;
                tour.Duration = model.Duration;
                tour.Badge = model.Badge;
                tour.Rating = model.Rating;
                tour.CategoryId = model.CategoryId;
                tour.DestinationId = model.DestinationId;

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    var existing = await _context.TourImages.FirstOrDefaultAsync(i => i.TourId == id);
                    if (existing != null) existing.ImageUrl = imageUrl;
                    else _context.TourImages.Add(new TourImage { TourId = id, ImageUrl = imageUrl });
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật Tour thành công!";
                return RedirectToAction("Tours");
            }
            return RedirectToAction("Tours");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa Tour thành công!";
            }
            return RedirectToAction("Tours");
        }

        // ===== SCHEDULES =====
        [HttpPost]
        public async Task<IActionResult> AddSchedule(int tourId, DateTime startDate, int totalSeats)
        {
            _context.TourSchedules.Add(new TourSchedule
            {
                TourId = tourId,
                StartDate = startDate,
                TotalSeats = totalSeats,
                AvailableSeats = totalSeats
            });
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã thêm lịch khởi hành!";
            return RedirectToAction("EditTour", new { id = tourId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSchedule(int id, int tourId)
        {
            var s = await _context.TourSchedules.FindAsync(id);
            if (s != null) { _context.TourSchedules.Remove(s); await _context.SaveChangesAsync(); }
            return RedirectToAction("EditTour", new { id = tourId });
        }

        // ===== PROMO CODES =====
        public async Task<IActionResult> Promos()
        {
            var promos = await _context.PromoCodes.OrderByDescending(p => p.ValidUntil).ToListAsync();
            return View(promos);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromo(PromoCode model)
        {
            _context.PromoCodes.Add(model);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã tạo mã \"{model.Code}\" thành công!";
            return RedirectToAction("Promos");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePromo(int id)
        {
            var promo = await _context.PromoCodes.FindAsync(id);
            if (promo != null)
            {
                promo.IsActive = !promo.IsActive;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = promo.IsActive ? "Đã bật mã giảm giá." : "Đã tắt mã giảm giá.";
            }
            return RedirectToAction("Promos");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePromo(int id)
        {
            var promo = await _context.PromoCodes.FindAsync(id);
            if (promo != null) { _context.PromoCodes.Remove(promo); await _context.SaveChangesAsync(); }
            TempData["SuccessMessage"] = "Đã xóa mã giảm giá.";
            return RedirectToAction("Promos");
        }
    }
}
