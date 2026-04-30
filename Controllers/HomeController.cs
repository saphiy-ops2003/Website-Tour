using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;
using WebsiteTour.Models.Entities;
using WebsiteTour.Models.ViewModels;

namespace WebsiteTour.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var featuredTours = await _context.Tours
            .Include(t => t.Images)
            .OrderByDescending(t => t.Rating)
            .Take(3)
            .ToListAsync();
            
        return View(featuredTours);
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return RedirectToAction("Login");

        var model = new WebsiteTour.Models.ViewModels.ProfileViewModel
        {
            Username = user.Username,
            Email = user.Email
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Profile(WebsiteTour.Models.ViewModels.ProfileViewModel model)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return RedirectToAction("Login");

        // Update email
        user.Email = model.Email;

        // Change password if provided
        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            if (model.CurrentPassword != user.PasswordHash)
            {
                TempData["ErrorMessage"] = "Mật khẩu hiện tại không đúng.";
                ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
                return View(model);
            }
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp.";
                ModelState.AddModelError("ConfirmNewPassword", "Mật khẩu xác nhận không khớp.");
                return View(model);
            }
            user.PasswordHash = model.NewPassword;
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
        
        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> Tours()
    {
        var tours = await _context.Tours
            .Include(t => t.Images)
            .Include(t => t.Destination)
            .ToListAsync();
            
        return View(tours);
    }

    public async Task<IActionResult> TourDetail(int id)
    {
        var tour = await _context.Tours
            .Include(t => t.Images)
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .Include(t => t.Itineraries.OrderBy(i => i.DayNumber))
            .Include(t => t.Schedules.Where(s => s.StartDate > DateTime.Now).OrderBy(s => s.StartDate))
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tour == null)
            return NotFound();
            
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdStr, out int userId))
        {
            var savedPromos = await _context.UserPromoCodes
                .Include(up => up.PromoCode)
                .Where(up => up.UserId == userId && !up.IsUsed && up.PromoCode!.IsActive && up.PromoCode.ValidUntil >= DateTime.Now)
                .Select(up => up.PromoCode)
                .ToListAsync();
            ViewBag.SavedPromos = savedPromos;
        }
        else
        {
            ViewBag.SavedPromos = new List<PromoCode>();
        }

        return View(tour);
    }

    public async Task<IActionResult> Deals()
    {
        var promos = await _context.PromoCodes.Where(p => p.IsActive && p.ValidUntil >= DateTime.Now).ToListAsync();
        
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
                    
                userPoints = (int)(totalSpent);
                
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

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email || u.Username == model.Username))
            {
                TempData["ErrorMessage"] = "Email hoặc Username đã tồn tại.";
                ModelState.AddModelError("", "Email hoặc Username đã tồn tại.");
                return View(model);
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
            if (role == null)
            {
                role = new Role { Name = "Customer" };
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                // In production, use proper password hashing
                PasswordHash = model.Password, 
                RoleId = role.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => (u.Email == model.EmailOrUsername || u.Username == model.EmailOrUsername) && u.PasswordHash == model.Password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role?.Name ?? "Customer")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                TempData["SuccessMessage"] = "Đăng nhập thành công! Chào mừng trở lại.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Tài khoản hoặc mật khẩu không đúng.";
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        TempData["SuccessMessage"] = "Bạn đã đăng xuất thành công.";
        return RedirectToAction("Index");
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
    public async Task<IActionResult> Checkout(int tourId, int adults = 2, int children = 0, string promoCode = "", int scheduleId = 0)
    {
        var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == tourId);
        if (tour == null) return RedirectToAction("Tours");
        
        var schedule = await _context.TourSchedules.FirstOrDefaultAsync(s => s.Id == scheduleId && s.TourId == tourId);
        if (schedule == null) return RedirectToAction("TourDetail", new { id = tourId });
        
        int totalGuests = adults + children;
        if (schedule.AvailableSeats < totalGuests)
        {
            TempData["ErrorMessage"] = $"Xin lỗi, lịch trình này chỉ còn {schedule.AvailableSeats} chỗ trống.";
            return RedirectToAction("TourDetail", new { id = tourId });
        }
        
        decimal discount = 0;
        string validPromoCode = "";
        
        if (!string.IsNullOrEmpty(promoCode))
        {
            decimal subTotal = (tour.Price * adults) + (tour.Price * 0.7m * children);
            var promo = await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == promoCode.ToUpper() && p.IsActive && p.ValidUntil >= DateTime.Now);
                
            if (promo != null && subTotal >= promo.MinOrderValue)
            {
                discount = promo.DiscountAmount;
                validPromoCode = promo.Code;
            }
        }
        
        ViewBag.Adults = adults;
        ViewBag.Children = children;
        ViewBag.PromoCode = validPromoCode;
        ViewBag.Discount = discount;
        ViewBag.Schedule = schedule;

        return View(tour);
    }

    [HttpPost]
    [Authorize]
    [ActionName("Checkout")]
    public async Task<IActionResult> CheckoutPost(int tourId, int adults, int children, string promoCode, int scheduleId)
    {
        var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == tourId);
        if (tour == null) return RedirectToAction("Tours");

        var schedule = await _context.TourSchedules.FirstOrDefaultAsync(s => s.Id == scheduleId && s.TourId == tourId);
        if (schedule == null) return RedirectToAction("TourDetail", new { id = tourId });

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        decimal adultPrice = tour.Price;
        decimal childPrice = tour.Price * 0.7m; // Children get 30% off
        decimal subTotal = (adultPrice * adults) + (childPrice * children);
        
        int totalGuests = adults + children;
        if (schedule.AvailableSeats < totalGuests)
        {
            TempData["ErrorMessage"] = $"Lịch trình này chỉ còn {schedule.AvailableSeats} chỗ trống.";
            return RedirectToAction("TourDetail", new { id = tourId });
        }
        
        decimal discount = 0;
        if (!string.IsNullOrEmpty(promoCode))
        {
            var promo = await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == promoCode.ToUpper() && p.IsActive && p.ValidUntil >= DateTime.Now);
                
            if (promo != null && subTotal >= promo.MinOrderValue)
            {
                discount = promo.DiscountAmount;
                
                // Mark as used if they had it saved
                var savedPromo = await _context.UserPromoCodes
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PromoCodeId == promo.Id && !up.IsUsed);
                if (savedPromo != null)
                {
                    savedPromo.IsUsed = true;
                }
            }
            else
            {
                promoCode = ""; // Invalid or not meeting minimum requirement
            }
        }
        
        schedule.AvailableSeats -= totalGuests; // Deduct seats

        var booking = new Booking
        {
            TourId = tour.Id,
            TourScheduleId = scheduleId,
            UserId = userId,
            BookingDate = DateTime.Now,
            AdultCount = adults,
            ChildCount = children,
            DiscountAmount = discount,
            PromoCode = promoCode,
            TotalPrice = subTotal - discount + 10, // Including $10 service fee
            Status = "Pending"
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Đặt Tour thành công! Cảm ơn bạn.";
        return RedirectToAction("Bookings");
    }

    [Authorize]
    public async Task<IActionResult> Bookings()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        var bookings = await _context.Bookings
            .Include(b => b.Tour)
            .ThenInclude(t => t.Images)
            .Include(b => b.Tour.Destination)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

        return View(bookings);
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
