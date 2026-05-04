using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;

namespace WebsiteTour.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> CreateTour(Tour model, List<IFormFile>? imageFiles, List<int> destinationIds, List<Itinerary> itineraries, List<TourSchedule> schedules)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description) || !destinationIds.Any() || itineraries == null || itineraries.Count < 1 || schedules == null || schedules.Count < 1 || model.Days < 1)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng nhập đầy đủ: Tên tour, Mô tả, ít nhất 1 điểm đến, 1 ngày lịch trình và 1 ngày khởi hành.";
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
            
            // Clear collections to prevent double-adding by EF (as they are also in parameters)
            model.Itineraries.Clear();
            model.Schedules.Clear();
            model.TourDestinations.Clear();

            _context.Tours.Add(model);
            await _context.SaveChangesAsync();

            // Save Itineraries
            foreach (var item in itineraries)
            {
                item.TourId = model.Id;
            }
            _context.Itineraries.AddRange(itineraries);

            // Save Schedules
            foreach (var s in schedules)
            {
                s.TourId = model.Id;
                s.AvailableSeats = s.TotalSeats; 
            }
            _context.TourSchedules.AddRange(schedules);

            if (imageFiles != null && imageFiles.Count > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                
                bool isFirst = true;
                foreach (var file in imageFiles)
                {
                    if (file.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        _context.TourImages.Add(new TourImage { TourId = model.Id, ImageUrl = "/uploads/" + uniqueFileName, IsPrimary = isFirst });
                        isFirst = false;
                    }
                }
            }

            var links = destinationIds.Distinct().Select(destinationId => new TourDestination
            {
                TourId = model.Id,
                DestinationId = destinationId
            });
            _context.TourDestinations.AddRange(links);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã tạo Tour \"{model.Title}\" thành công!";
            return RedirectToAction("Tours");
        }

        public async Task<IActionResult> EditTour(int id)
        {
            var tour = await _context.Tours
                .Include(t => t.Images)
                .Include(t => t.Schedules.OrderBy(s => s.StartDate))
                .Include(t => t.TourDestinations)
                .Include(t => t.Itineraries.OrderBy(i => i.DayNumber))
                .FirstOrDefaultAsync(t => t.Id == id);
            if (tour == null) return RedirectToAction("Tours");

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Destinations = await _context.Destinations.Include(d => d.Region).ToListAsync();
            ViewBag.Regions = await _context.Regions.OrderBy(r => r.Name).ToListAsync();
            ViewBag.SelectedDestinationIds = tour.TourDestinations.Select(td => td.DestinationId).ToList();
            return View(tour);
        }

        [HttpPost]
        public async Task<IActionResult> EditTour(int id, Tour model, List<IFormFile>? imageFiles, List<int> destinationIds, List<Itinerary> itineraries, List<TourSchedule> schedules)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour != null)
            {
                if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description) || !destinationIds.Any() || itineraries == null || itineraries.Count < 1 || schedules == null || schedules.Count < 1 || model.Days < 1)
                {
                    TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng nhập đầy đủ: Tên tour, Mô tả, ít nhất 1 điểm đến, 1 ngày lịch trình và 1 ngày khởi hành.";
                    return RedirectToAction("EditTour", new { id });
                }

                tour.Title = model.Title;
                tour.Description = model.Description;
                tour.Price = model.Price;
                tour.Days = model.Days;
                tour.Nights = model.Nights;
                tour.Duration = $"{model.Days} Ngày {model.Nights} Đêm";
                tour.DestinationId = destinationIds.First();

                // Update Itineraries
                var oldItineraries = await _context.Itineraries.Where(i => i.TourId == id).ToListAsync();
                _context.Itineraries.RemoveRange(oldItineraries);
                foreach (var item in itineraries)
                {
                    item.TourId = id;
                    item.Id = 0;
                }
                _context.Itineraries.AddRange(itineraries);

                // Update Schedules
                var oldSchedules = await _context.TourSchedules.Where(s => s.TourId == id).ToListAsync();
                _context.TourSchedules.RemoveRange(oldSchedules);
                foreach (var s in schedules)
                {
                    s.TourId = id;
                    s.Id = 0;
                    if (s.AvailableSeats == 0) s.AvailableSeats = s.TotalSeats;
                }
                _context.TourSchedules.AddRange(schedules);

                if (imageFiles != null && imageFiles.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var existingImages = await _context.TourImages.Where(i => i.TourId == id).ToListAsync();
                    bool hasPrimary = existingImages.Any(i => i.IsPrimary);

                    foreach (var file in imageFiles)
                    {
                        if (file.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                            
                            _context.TourImages.Add(new TourImage { TourId = id, ImageUrl = "/uploads/" + uniqueFileName, IsPrimary = !hasPrimary });
                            hasPrimary = true;
                        }
                    }
                }

                var oldLinks = await _context.TourDestinations.Where(td => td.TourId == id).ToListAsync();
                _context.TourDestinations.RemoveRange(oldLinks);
                _context.TourDestinations.AddRange(
                    destinationIds.Distinct().Select(destinationId => new TourDestination
                    {
                        TourId = id,
                        DestinationId = destinationId
                    })
                );

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
                bool hasBookings = await _context.Bookings.AnyAsync(b => b.TourId == id);
                if (hasBookings)
                {
                    // Soft delete
                    tour.IsDeleted = true;
                    _context.Tours.Update(tour);
                    TempData["SuccessMessage"] = "Tour đã có booking nên được chuyển sang trạng thái ẩn (Soft Delete) để bảo toàn dữ liệu!";
                }
                else
                {
                    // Hard delete
                    _context.Tours.Remove(tour);
                    TempData["SuccessMessage"] = "Đã xóa Tour vĩnh viễn thành công!";
                }
                await _context.SaveChangesAsync();
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

        // ===== REGIONS & DESTINATIONS =====
        public async Task<IActionResult> Destinations()
        {
            var destinations = await _context.Destinations
                .Include(d => d.Region)
                .Include(d => d.TourDestinations)
                .OrderBy(d => d.Region!.Name)
                .ThenBy(d => d.Name)
                .ToListAsync();

            ViewBag.Regions = await _context.Regions
                .OrderBy(r => r.Name)
                .ToListAsync();

            return View("DestinationsManage", destinations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDestination(string name, string description, IFormFile? imageFile, int regionId, string? slug)
        {
            if (string.IsNullOrWhiteSpace(name) || regionId <= 0)
            {
                TempData["ErrorMessage"] = "Tên điểm đến và vùng miền là bắt buộc.";
                return RedirectToAction("Destinations");
            }

            var finalSlug = BuildSlug(string.IsNullOrWhiteSpace(slug) ? name : slug);
            var exists = await _context.Destinations.AnyAsync(d => d.Slug == finalSlug || d.Name.ToLower() == name.Trim().ToLower());
            if (exists)
            {
                TempData["ErrorMessage"] = $"Tên điểm đến \"{name.Trim()}\" hoặc đường dẫn đã tồn tại trong hệ thống.";
                return RedirectToAction("Destinations");
            }

            string imageUrl = "";
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                imageUrl = "/uploads/" + uniqueFileName;
            }

            _context.Destinations.Add(new Destination
            {
                Name = name.Trim(),
                Description = description?.Trim() ?? string.Empty,
                ImageUrl = imageUrl,
                RegionId = regionId,
                Slug = finalSlug
            });
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã thêm điểm đến mới.";
            return RedirectToAction("Destinations");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDestination(int id, string name, string description, IFormFile? imageFile, int regionId, string slug)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy điểm đến.";
                return RedirectToAction("Destinations");
            }

            if (string.IsNullOrWhiteSpace(name) || regionId <= 0 || string.IsNullOrWhiteSpace(slug))
            {
                TempData["ErrorMessage"] = "Tên điểm đến và vùng miền là bắt buộc.";
                return RedirectToAction("Destinations");
            }

            var finalSlug = BuildSlug(slug);
            var slugExists = await _context.Destinations.AnyAsync(d => d.Id != id && (d.Slug == finalSlug || d.Name.ToLower() == name.Trim().ToLower()));
            if (slugExists)
            {
                TempData["ErrorMessage"] = $"Tên điểm đến \"{name.Trim()}\" hoặc đường dẫn đã tồn tại trong hệ thống.";
                return RedirectToAction("Destinations");
            }

            destination.Name = name.Trim();
            destination.Description = description?.Trim() ?? string.Empty;
            
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                destination.ImageUrl = "/uploads/" + uniqueFileName;
            }

            destination.RegionId = regionId;
            destination.Slug = finalSlug;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã cập nhật điểm đến.";
            return RedirectToAction("Destinations");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var destination = await _context.Destinations
                .Include(d => d.Tours)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy điểm đến.";
                return RedirectToAction("Destinations");
            }

            if (destination.Tours.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa điểm đến đang có tour. Hãy chuyển tour sang điểm đến khác trước.";
                return RedirectToAction("Destinations");
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã xóa điểm đến.";
            return RedirectToAction("Destinations");
        }

        public async Task<IActionResult> Regions()
        {
            var regions = await _context.Regions
                .Include(r => r.Destinations)
                .OrderBy(r => r.Name)
                .ToListAsync();
            return View(regions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion(string name, string? slug)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["ErrorMessage"] = "Tên vùng là bắt buộc.";
                return RedirectToAction("Regions");
            }

            var finalSlug = BuildSlug(string.IsNullOrWhiteSpace(slug) ? name : slug);
            var exists = await _context.Regions.AnyAsync(r => r.Slug == finalSlug || r.Name.ToLower() == name.Trim().ToLower());
            if (exists)
            {
                TempData["ErrorMessage"] = $"Tên vùng \"{name.Trim()}\" hoặc đường dẫn đã tồn tại trong hệ thống.";
                return RedirectToAction("Regions");
            }

            _context.Regions.Add(new Region { Name = name.Trim(), Slug = finalSlug });
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã thêm vùng mới.";
            return RedirectToAction("Regions");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRegion(int id, string name, string slug)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy vùng.";
                return RedirectToAction("Regions");
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(slug))
            {
                TempData["ErrorMessage"] = "Tên vùng và slug là bắt buộc.";
                return RedirectToAction("Regions");
            }

            var finalSlug = BuildSlug(slug);
            var exists = await _context.Regions.AnyAsync(r => r.Id != id && (r.Slug == finalSlug || r.Name.ToLower() == name.Trim().ToLower()));
            if (exists)
            {
                TempData["ErrorMessage"] = $"Tên vùng \"{name.Trim()}\" hoặc đường dẫn đã tồn tại trong hệ thống.";
                return RedirectToAction("Regions");
            }

            region.Name = name.Trim();
            region.Slug = finalSlug;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã cập nhật vùng.";
            return RedirectToAction("Regions");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            var region = await _context.Regions
                .Include(r => r.Destinations)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (region == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy vùng.";
                return RedirectToAction("Regions");
            }

            if (region.Destinations.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa vùng đang có điểm đến.";
                return RedirectToAction("Regions");
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã xóa vùng.";
            return RedirectToAction("Regions");
        }

        private static string BuildSlug(string input)
        {
            var lower = input.Trim().ToLowerInvariant();
            var normalized = lower
                .Replace("đ", "d")
                .Normalize(System.Text.NormalizationForm.FormD);
            var cleaned = new string(normalized.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray());
            var slug = Regex.Replace(cleaned, @"[^a-z0-9]+", "-").Trim('-');
            return string.IsNullOrWhiteSpace(slug) ? "n-a" : slug;
        }
    }
}
