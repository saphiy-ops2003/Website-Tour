using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebsiteTour.Models;

public class PromotionController : Controller
{
    private readonly AppDbContext _context;

    public PromotionController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Deals()
    {
        var promos = await _context.PromoCodes
            .Where(p => p.IsActive && p.ValidUntil >= DateTime.Now)
            .ToListAsync();

        var latestDeal = await _context.Deals
            .Include(d => d.Tour)
            .Where(d => d.ValidUntil >= DateTime.Now)
            .OrderBy(d => d.ValidUntil)
            .FirstOrDefaultAsync();

        ViewBag.LatestDeal = latestDeal;

        int userPoints = 0;
        string cardName = "KHÁCH VÃNG LAI";
        string tier = "MEMBER";

        if (User.Identity?.IsAuthenticated == true)
        {
            cardName = User.Identity.Name?.ToUpper() ?? "THÀNH VIÊN";
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdStr, out int userId))
            {
                var totalSpent = await _context.Bookings
                    .Where(b => b.UserId == userId)
                    .SumAsync(b => b.TotalPrice);

                userPoints = (int)totalSpent;

                if (userPoints > 5000) tier = "DIAMOND";
                else if (userPoints > 2000) tier = "PLATINUM";
                else if (userPoints > 500) tier = "GOLD";
            }
        }

        ViewBag.UserPoints = userPoints;
        ViewBag.CardName = cardName;
        ViewBag.Tier = tier;

        return View(promos);
    }
}