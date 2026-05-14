using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;

namespace WebsiteTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ToursAdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ToursAdminController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // ===== TOURS =====
        public async Task<IActionResult> Index()
        {
            var tours = await _context.Tours
                .Include(t => t.Images)
                .Include(t => t.Destination)
                .Include(t => t.TourDestinations)
                .ThenInclude(td => td.Destination)
                .Include(t => t.Category)
                .Include(t => t.Schedules)
                .Include(t => t.Bookings)
                .ToListAsync();
            return View(tours);
        }

        public async Task<IActionResult> CreateTour()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Destinations = await _context.Destinations.Include(d => d.Region).ToListAsync();
            ViewBag.Regions = await _context.Regions.OrderBy(r => r.Name).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(
            Tour model,
            List<IFormFile>? imageFiles,
            List<int> destinationIds,
            List<Itinerary> itineraries,
            List<TourSchedule> schedules)
        {
            if (string.IsNullOrWhiteSpace(model.Title)
                || string.IsNullOrWhiteSpace(model.Description)
                || !destinationIds.Any()
                || itineraries == null || itineraries.Count < 1
                || schedules == null || schedules.Count < 1
                || model.Days < 1)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ.";
                ViewBag.Categories = await _context.Categories.ToListAsync();
                ViewBag.Destinations = await _context.Destinations.Include(d => d.Region).ToListAsync();
                ViewBag.Regions = await _context.Regions.OrderBy(r => r.Name).ToListAsync();
                return View(model);
            }

            model.Duration = $"{model.Days} Ngày {model.Nights} Đêm";
            model.DestinationId = destinationIds.First();

            if (model.CategoryId == 0)
            {
                var defaultCategory = await _context.Categories.FirstOrDefaultAsync();
                model.CategoryId = defaultCategory?.Id ?? 1;
            }

            model.Rating = 0;
            model.TotalReviews = 0;

            model.Itineraries.Clear();
            model.Schedules.Clear();
            model.TourDestinations.Clear();

            _context.Tours.Add(model);
            await _context.SaveChangesAsync();

            // Itinerary
            foreach (var item in itineraries)
            {
                item.TourId = model.Id;
            }
            _context.Itineraries.AddRange(itineraries);

            // Schedule
            foreach (var s in schedules)
            {
                s.TourId = model.Id;
                s.AvailableSeats = s.TotalSeats;
            }
            _context.TourSchedules.AddRange(schedules);

            // Images
            if (imageFiles != null && imageFiles.Count > 0)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                bool isFirst = true;
                foreach (var file in imageFiles)
                {
                    if (file.Length > 0)
                    {
                        string fileName = Guid.NewGuid() + "_" + file.FileName;
                        string path = Path.Combine(folder, fileName);

                        using var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);

                        _context.TourImages.Add(new TourImage
                        {
                            TourId = model.Id,
                            ImageUrl = "/uploads/" + fileName,
                            IsPrimary = isFirst
                        });

                        isFirst = false;
                    }
                }
            }

            // Destinations
            var links = destinationIds.Distinct().Select(d => new TourDestination
            {
                TourId = model.Id,
                DestinationId = d
            });
            _context.TourDestinations.AddRange(links);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã tạo Tour \"{model.Title}\"";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditTour(int id)
        {
            var tour = await _context.Tours
                .Include(t => t.Images)
                .Include(t => t.Schedules.OrderBy(s => s.StartDate))
                .Include(t => t.TourDestinations)
                .Include(t => t.Itineraries.OrderBy(i => i.DayNumber))
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tour == null) return RedirectToAction("Index");

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Destinations = await _context.Destinations.Include(d => d.Region).ToListAsync();
            ViewBag.Regions = await _context.Regions.OrderBy(r => r.Name).ToListAsync();
            ViewBag.SelectedDestinationIds = tour.TourDestinations.Select(td => td.DestinationId).ToList();

            return View(tour);
        }

        [HttpPost]
        public async Task<IActionResult> EditTour(
            int id,
            Tour model,
            List<IFormFile>? imageFiles,
            List<int> destinationIds,
            List<Itinerary> itineraries,
            List<TourSchedule> schedules)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null) return RedirectToAction("Tours");

            if (string.IsNullOrWhiteSpace(model.Title)
                || string.IsNullOrWhiteSpace(model.Description)
                || !destinationIds.Any()
                || itineraries.Count < 1
                || schedules.Count < 1)
            {
                TempData["ErrorMessage"] = "Thiếu dữ liệu!";
                return RedirectToAction("EditTour", new { id });
            }

            tour.Title = model.Title;
            tour.Description = model.Description;
            tour.Price = model.Price;
            tour.Days = model.Days;
            tour.Nights = model.Nights;
            tour.Duration = $"{model.Days} Ngày {model.Nights} Đêm";
            tour.DestinationId = destinationIds.First();

            // Itinerary
            var oldIt = await _context.Itineraries.Where(i => i.TourId == id).ToListAsync();
            _context.Itineraries.RemoveRange(oldIt);
            foreach (var item in itineraries)
            {
                item.TourId = id;
                item.Id = 0;
            }
            _context.Itineraries.AddRange(itineraries);

            // Schedule
foreach (var s in schedules)
{
    // schedule mới
    if (s.Id == 0)
    {
        s.TourId = id;

        if (s.AvailableSeats == 0)
            s.AvailableSeats = s.TotalSeats;

        _context.TourSchedules.Add(s);
    }
    else
    {
        // schedule cũ
        var existingSchedule = await _context.TourSchedules.FindAsync(s.Id);

        if (existingSchedule != null)
        {
            existingSchedule.StartDate = s.StartDate;
            existingSchedule.TotalSeats = s.TotalSeats;

            // tránh available > total
            if (existingSchedule.AvailableSeats > existingSchedule.TotalSeats)
            {
                existingSchedule.AvailableSeats = existingSchedule.TotalSeats;
            }
        }
    }
}

            // Images
            if (imageFiles != null && imageFiles.Count > 0)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                foreach (var file in imageFiles)
                {
                    if (file.Length > 0)
                    {
                        string fileName = Guid.NewGuid() + "_" + file.FileName;
                        string path = Path.Combine(folder, fileName);

                        using var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);

                        _context.TourImages.Add(new TourImage
                        {
                            TourId = id,
                            ImageUrl = "/uploads/" + fileName
                        });
                    }
                }
            }

            // Destinations
            var oldLinks = await _context.TourDestinations.Where(td => td.TourId == id).ToListAsync();
            _context.TourDestinations.RemoveRange(oldLinks);
            _context.TourDestinations.AddRange(
                destinationIds.Select(d => new TourDestination
                {
                    TourId = id,
                    DestinationId = d
                }));

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTour(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                bool hasBooking = await _context.Bookings.AnyAsync(b => b.TourId == id);

                if (hasBooking)
                {
                    tour.IsDeleted = true;
                    _context.Tours.Update(tour);
                }
                else
                {
                    _context.Tours.Remove(tour);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // ===== SCHEDULE =====
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
            return RedirectToAction("EditTour", new { id = tourId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSchedule(int id, int tourId)
        {
            var s = await _context.TourSchedules.FindAsync(id);
            if (s != null)
            {
                _context.TourSchedules.Remove(s);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("EditTour", new { id = tourId });
        }
    }
}