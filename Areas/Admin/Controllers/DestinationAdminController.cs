using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;
using WebsiteTour.Services;

namespace WebsiteTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DestinationAdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SlugService _slugService;


        public DestinationAdminController(AppDbContext context, IWebHostEnvironment webHostEnvironment, SlugService slugService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _slugService = slugService;
        }

        // ===== LIST =====
        public async Task<IActionResult> Index()
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

            return View(destinations);
        }

        // ===== CREATE =====
        [HttpPost]
        public async Task<IActionResult> Create(string name, string description, IFormFile? imageFile, int regionId, string? slug)
        {
            if (string.IsNullOrWhiteSpace(name) || regionId <= 0)
            {
                TempData["ErrorMessage"] = "Tên điểm đến và vùng miền là bắt buộc.";
                return RedirectToAction("Index");
            }

            var finalSlug = BuildSlug(string.IsNullOrWhiteSpace(slug) ? name : slug);

            var exists = await _context.Destinations
                .AnyAsync(d => d.Slug == finalSlug || d.Name.ToLower() == name.Trim().ToLower());

            if (exists)
            {
                TempData["ErrorMessage"] = $"Tên điểm đến \"{name.Trim()}\" đã tồn tại.";
                return RedirectToAction("Index");
            }

            string imageUrl = "";

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid() + "_" + imageFile.FileName;
                string path = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                imageUrl = "/uploads/" + fileName;
            }

            _context.Destinations.Add(new Destination
            {
                Name = name.Trim(),
                Description = description?.Trim() ?? "",
                ImageUrl = imageUrl,
                RegionId = regionId,
                Slug = finalSlug
            });

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã thêm điểm đến.";
            return RedirectToAction("Index");
        }

        // ===== UPDATE =====
        [HttpPost]
        public async Task<IActionResult> Update(int id, string name, string description, IFormFile? imageFile, int regionId, string slug)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy điểm đến.";
                return RedirectToAction("Index");
            }

            var finalSlug = BuildSlug(slug);

            var exists = await _context.Destinations
                .AnyAsync(d => d.Id != id && (d.Slug == finalSlug || d.Name.ToLower() == name.Trim().ToLower()));

            if (exists)
            {
                TempData["ErrorMessage"] = "Slug hoặc tên đã tồn tại.";
                return RedirectToAction("Index");
            }

            destination.Name = name.Trim();
            destination.Description = description?.Trim() ?? "";
            destination.RegionId = regionId;
            destination.Slug = finalSlug;

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid() + "_" + imageFile.FileName;
                string path = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                destination.ImageUrl = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã cập nhật.";
            return RedirectToAction("Index");
        }

        // ===== DELETE =====
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var destination = await _context.Destinations
                .Include(d => d.Tours)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy.";
                return RedirectToAction("Index");
            }

            if (destination.Tours.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa vì đang có tour.";
                return RedirectToAction("Index");
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã xóa.";
            return RedirectToAction("Index");
        }

        // ===== HELPER =====
        private string BuildSlug(string input)
        {
            string str = input.ToLower();
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", "-").Trim();
            return str;
        }
    }
}