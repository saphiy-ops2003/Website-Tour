using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebsiteTour.Models;

public class TourController : Controller
{
    private readonly AppDbContext _context;

    public TourController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Tours(int? regionId = null, int? destinationId = null, decimal? maxPrice = null, int? duration = null, string sortBy = "popular", int page = 1)
    {
        var query = _context.Tours
            .Where(t => !t.IsDeleted)
            .Include(t => t.Images)
            .Include(t => t.Destination)
            .AsQueryable();

        if (destinationId.HasValue)
        {
            query = query.Where(t => t.TourDestinations.Any(td => td.DestinationId == destinationId.Value));

            var destination = await _context.Destinations.FirstOrDefaultAsync(d => d.Id == destinationId.Value);
            if (destination != null) ViewBag.FilterDestination = destination.Name;
        }

        if (regionId.HasValue)
        {
            query = query.Where(t => t.TourDestinations.Any(td => td.Destination != null && td.Destination.RegionId == regionId.Value));
        }

        if (maxPrice.HasValue && maxPrice.Value > 0)
        {
            query = query.Where(t => t.Price <= maxPrice.Value);
        }

        if (duration.HasValue)
        {
            if (duration == 1) query = query.Where(t => t.Days >= 1 && t.Days <= 3);
            else if (duration == 2) query = query.Where(t => t.Days >= 4 && t.Days <= 7);
            else if (duration == 3) query = query.Where(t => t.Days > 7);
        }

        switch (sortBy)
        {
            case "price_asc":
                query = query.OrderBy(t => t.Price);
                break;
            case "price_desc":
                query = query.OrderByDescending(t => t.Price);
                break;
            default:
                query = query.OrderByDescending(t => t.Bookings.Count).ThenByDescending(t => t.Rating);
                break;
        }

        int pageSize = 9;
        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

        var tours = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsSplitQuery()
            .ToListAsync();

        ViewBag.Regions = await _context.Regions.OrderBy(r => r.Name).ToListAsync();
        ViewBag.CurrentRegion = regionId;
        ViewBag.MaxPrice = maxPrice;
        ViewBag.CurrentDuration = duration;
        ViewBag.SortBy = sortBy;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalItems = totalItems;

        return View(tours);
    }

    public async Task<IActionResult> TourDetail(int id)
    {
        var tour = await _context.Tours
            .Where(t => !t.IsDeleted)
            .Include(t => t.Images)
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .Include(t => t.Itineraries.OrderBy(i => i.DayNumber))
            .Include(t => t.Schedules.Where(s => s.StartDate > DateTime.Now).OrderBy(s => s.StartDate))
            .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tour == null)
            return NotFound();

        return View(tour);
    }

    [Authorize]
    public async Task<IActionResult> RecommendedTours(int topN = 6)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        var tours = await _context.Tours
            .Where(t => !t.IsDeleted)
            .Include(t => t.Images)
            .Include(t => t.Destination)
            .OrderByDescending(t => t.Bookings.Count)
            .ThenByDescending(t => t.Rating)
            .Take(topN)
            .ToListAsync();

        return View(tours);
    }
}