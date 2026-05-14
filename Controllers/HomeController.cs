using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;
using WebsiteTour.Models.ViewModels;
using WebsiteTour.Services;
using WebsiteTour.ViewModels;

namespace WebsiteTour.Controllers;

    // Controller for public-facing website pages and user actions
    public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly IRecommendationClient _recommendationClient;

    public HomeController(
        ILogger<HomeController> logger,
        AppDbContext context,
        IRecommendationClient recommendationClient
    )
    {
        _logger = logger;
        _context = context;
        _recommendationClient = recommendationClient;
    }

   public async Task<IActionResult> Index()
{
    var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
    int.TryParse(userIdStr, out int userId);

    int? selectedRegionId = null;
    if (int.TryParse(Request.Query["regionId"], out int regionId))
    {
        selectedRegionId = regionId;
    }

    var destinationRegions = await BuildDestinationByRegionAsync();
    selectedRegionId ??= destinationRegions.FirstOrDefault()?.RegionId;

    var model = new HomeIndexViewModel
    {
        PopularTours = await BuildPopularToursAsync(6),
        RecommendedTours = await BuildRecommendedToursAsync(6),
        DestinationRegions = destinationRegions,
        SelectedRegionId = selectedRegionId
    };

    return View(model);
}

    private async Task<List<Tour>> BuildRecommendedToursAsync(int topN)
{
    return await _context.Tours
        .Where(t => !t.IsDeleted)
        .Include(t => t.Images)
        .Include(t => t.Destination)
        .OrderByDescending(t => t.Rating)   // gợi ý theo rating cao
        .ThenByDescending(t => t.Bookings.Count)
        .Take(topN)
        .ToListAsync();
}

    private async Task<List<Tour>> BuildPopularToursAsync(int topN)
    {
        return await _context.Tours
            .Where(t => !t.IsDeleted)
            .Include(t => t.Images)
            .Include(t => t.Destination)
            .Include(t => t.TourDestinations)
            .ThenInclude(td => td.Destination)
            .OrderByDescending(t => t.Bookings.Count)
            .ThenByDescending(t => t.Rating)
            .AsSplitQuery()
            .Take(topN)
            .ToListAsync();
    }

    private async Task<List<DestinationRegionGroupViewModel>> BuildDestinationByRegionAsync()
    {
        var regions = await _context.Regions
            .Include(r => r.Destinations)
            .ThenInclude(d => d.TourDestinations.Where(td => !td.Tour.IsDeleted))
            .ThenInclude(td => td.Tour)
            .ThenInclude(t => t!.Bookings)
            .AsSplitQuery()
            .ToListAsync();

        return regions
            .Select(r => new DestinationRegionGroupViewModel
            {
                RegionId = r.Id,
                RegionName = r.Name,
                RegionSlug = r.Slug,  
                Destinations = r.Destinations
                    .OrderByDescending(d => d.TourDestinations.Sum(td => td.Tour?.Bookings.Count ?? 0))
                    .ThenBy(d => d.Name)
                    .ToList()
            })
            .OrderBy(g => g.RegionName)
            .ToList();
    }

    [HttpGet]
    public async Task<IActionResult> ValidatePromo(string code, decimal total)
    {
        var promo = await _context.PromoCodes
            .FirstOrDefaultAsync(p => p.Code == code.ToUpper() && p.IsActive && p.ValidUntil >= DateTime.Now);

        if (promo == null)
            return Json(new { success = false, message = "Mã giảm giá không hợp lệ hoặc đã hết hạn." });

        if (total < promo.MinOrderValue)
            return Json(new { success = false, message = $"Mã này yêu cầu đơn hàng tối thiểu ${promo.MinOrderValue}." });

        return Json(new { success = true, discount = promo.DiscountAmount, code = promo.Code, message = "Áp dụng mã giảm giá thành công!" });
    }

    [Authorize]
    public async Task<IActionResult> RecommendedTours(int topN = 6, CancellationToken cancellationToken = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var recommendedIds = await _recommendationClient.GetRecommendedTourIdsAsync(userId, topN, cancellationToken);

        List<Tour> tours;
        if (recommendedIds.Count > 0)
        {
            tours = await _context.Tours
                .Where(t => recommendedIds.Contains(t.Id) && !t.IsDeleted)
                .Include(t => t.Images)
                .Include(t => t.Destination)
                .ToListAsync(cancellationToken);

            var rankMap = recommendedIds
                .Select((id, index) => new { id, index })
                .ToDictionary(x => x.id, x => x.index);
            tours = tours.OrderBy(t => rankMap.TryGetValue(t.Id, out var rank) ? rank : int.MaxValue).ToList();
            ViewBag.RecommendationSource = "ai";
        }
        else
        {
            tours = await _context.Tours
                .Where(t => !t.IsDeleted)
                .Include(t => t.Images)
                .Include(t => t.Destination)
                .OrderByDescending(t => t.Bookings.Count)
                .ThenByDescending(t => t.Rating)
                .Take(topN)
                .ToListAsync(cancellationToken);
            ViewBag.RecommendationSource = "fallback";
        }

        return View(tours);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SavePromoCode([FromBody] SavePromoRequest req)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out int userId))
            return Json(new { success = false, message = "Bạn cần đăng nhập để lưu mã." });
            
        var promo = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == req.Code.ToUpper() && p.IsActive && p.ValidUntil >= DateTime.Now);
        
        if (promo == null)
            return Json(new { success = false, message = "Mã giảm giá không tồn tại hoặc đã hết hạn." });
            
        bool alreadySaved = await _context.UserPromoCodes.AnyAsync(up => up.UserId == userId && up.PromoCodeId == promo.Id);
        if (alreadySaved)
            return Json(new { success = false, message = "Bạn đã lưu mã này rồi." });
            
        _context.UserPromoCodes.Add(new UserPromoCode { UserId = userId, PromoCodeId = promo.Id });
        await _context.SaveChangesAsync();
        
        return Json(new { success = true, message = "Đã lưu mã giảm giá vào ví voucher của bạn!" });
    }
}

public class SavePromoRequest
{
    public string Code { get; set; } = string.Empty;
}
