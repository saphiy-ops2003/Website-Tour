using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;

namespace WebsiteTour.Areas.Admin.Controllers
{
        [Area("Admin")]
    public class BookingAdminController : Controller
    {
        private readonly AppDbContext _context;

        public BookingAdminController(AppDbContext context)
        {
            _context = context;
        }

        // ===== BOOKINGS =====
        public async Task<IActionResult> Index()
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

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Tour)
                .Include(b => b.TourSchedule)
                .FirstOrDefaultAsync(b => b.Id == id);
        
            if (booking == null)
                return RedirectToAction("Index");
        
            return View(booking);
        }
    }
}