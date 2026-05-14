using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;

namespace WebsiteTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromotionAdminController : Controller
    {
        private readonly AppDbContext _context;

        public PromotionAdminController(AppDbContext context)
        {
            _context = context;
        }

        // ===== PROMO CODES =====
        public async Task<IActionResult> Index()
        {
            var promos = await _context.PromoCodes
                .OrderByDescending(p => p.ValidUntil)
                .ToListAsync();

            return View(promos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromoCode model)
        {
            _context.PromoCodes.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã tạo mã \"{model.Code}\" thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var promo = await _context.PromoCodes.FindAsync(id);

            if (promo != null)
            {
                promo.IsActive = !promo.IsActive;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = promo.IsActive 
                    ? "Đã bật mã giảm giá." 
                    : "Đã tắt mã giảm giá.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var promo = await _context.PromoCodes.FindAsync(id);

            if (promo != null)
            {
                _context.PromoCodes.Remove(promo);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Đã xóa mã giảm giá.";
            return RedirectToAction("Index");
        }
    }
}