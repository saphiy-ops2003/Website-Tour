using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;
using WebsiteTour.Services;

namespace WebsiteTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegionAdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SlugService _slugService;

        public RegionAdminController(AppDbContext context, SlugService slugService)
        {
            _context = context;
            _slugService = slugService;
        }

        // ===== LIST =====
        public async Task<IActionResult> Index()
        {
            var regions = await _context.Regions
                .Include(r => r.Destinations)
                .OrderBy(r => r.Name)
                .ToListAsync();

            return View(regions);
        }

        // ===== CREATE =====
        [HttpPost]
        public async Task<IActionResult> Create(string name, string? slug)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["ErrorMessage"] = "Tên vùng là bắt buộc.";
                return RedirectToAction("Index");
            }

            var finalSlug = _slugService.BuildSlug(
    string.IsNullOrWhiteSpace(slug) ? name : slug
);

            var exists = await _context.Regions
                .AnyAsync(r => r.Slug == finalSlug || r.Name.ToLower() == name.Trim().ToLower());

            if (exists)
            {
                TempData["ErrorMessage"] = $"Tên vùng \"{name.Trim()}\" đã tồn tại.";
                return RedirectToAction("Index");
            }

            _context.Regions.Add(new Region
            {
                Name = name.Trim(),
                Slug = finalSlug
            });

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã thêm vùng.";
            return RedirectToAction("Index");
        }

        // ===== UPDATE =====
        [HttpPost]
        public async Task<IActionResult> Update(int id, string name, string slug)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy vùng.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(slug))
            {
                TempData["ErrorMessage"] = "Tên vùng và slug là bắt buộc.";
                return RedirectToAction("Index");
            }

            var finalSlug = _slugService.BuildSlug(
    string.IsNullOrWhiteSpace(slug) ? name : slug
);

            var exists = await _context.Regions
                .AnyAsync(r => r.Id != id && (r.Slug == finalSlug || r.Name.ToLower() == name.Trim().ToLower()));

            if (exists)
            {
                TempData["ErrorMessage"] = $"Tên vùng \"{name.Trim()}\" đã tồn tại.";
                return RedirectToAction("Index");
            }

            region.Name = name.Trim();
            region.Slug = finalSlug;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã cập nhật vùng.";
            return RedirectToAction("Index");
        }

        // ===== DELETE =====
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var region = await _context.Regions
                .Include(r => r.Destinations)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (region == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy vùng.";
                return RedirectToAction("Index");
            }

            if (region.Destinations.Any())
            {
                TempData["ErrorMessage"] = "Không thể xóa vì còn điểm đến.";
                return RedirectToAction("Index");
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã xóa vùng.";
            return RedirectToAction("Index");
        }

         // ===== HELPER =====
        // private string BuildSlug(string input)
        // {
        //     string str = input.ToLower();
        //     str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", "");
        //     str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", "-").Trim();
        //     return str;
        // }
    }
}