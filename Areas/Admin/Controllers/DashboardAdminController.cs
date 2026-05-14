using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;

namespace WebsiteTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardAdminController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardAdminController(AppDbContext context)
        {
            _context = context;
        }

        // ===== DASHBOARD =====
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalTours = await _context.Tours.CountAsync();
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();
            ViewBag.TotalRevenue = await _context.Bookings
                .Where(b => b.Status == "Confirmed")
                .SumAsync(b => b.TotalPrice);
            ViewBag.TotalUsers = await _context.Users.CountAsync();

            var recentBookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Tour)
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .ToListAsync();

            return View(recentBookings);
        }
    }
}