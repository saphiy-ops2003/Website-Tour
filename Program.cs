using Microsoft.EntityFrameworkCore;
using WebsiteTour.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "VietExploreAuth";
        options.LoginPath = "/Home/Login";
        options.LogoutPath = "/Home/Logout";
        options.AccessDeniedPath = "/Home/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.Migrate();

        if (!context.Tours.Any())
        {
            var category = new WebsiteTour.Models.Entities.Category { Name = "Khám phá" };
            var destination = new WebsiteTour.Models.Entities.Destination { Name = "Hạ Long", Region = "Miền Bắc" };
            var destination2 = new WebsiteTour.Models.Entities.Destination { Name = "Sapa", Region = "Miền Bắc" };

            context.Categories.Add(category);
            context.Destinations.AddRange(destination, destination2);
            context.SaveChanges();

            context.Tours.AddRange(
                new WebsiteTour.Models.Entities.Tour { Title = "Du thuyền Hạ Long Sang Trọng & Khám Phá Hang Động", Price = 299, Rating = 4.9, Duration = "3 Ngày 2 Đêm", Badge = "Bán Chạy Nhất", CategoryId = category.Id, DestinationId = destination.Id },
                new WebsiteTour.Models.Entities.Tour { Title = "Trekking Sapa: Trải nghiệm Homestay Bản Làng", Price = 185, Rating = 4.7, Duration = "4 Ngày 3 Đêm", CategoryId = category.Id, DestinationId = destination2.Id }
            );
            context.SaveChanges();

            var tour1 = context.Tours.First();
            var tour2 = context.Tours.Skip(1).First();
            context.TourImages.AddRange(
                new WebsiteTour.Models.Entities.TourImage { TourId = tour1.Id, ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuCJTc_bIx0R9u9bMrsmfty95mpAXFuDUqa1VYdHkrVHgd_74X7EqkUAHVcun8lVCF3TysT9-0qO_Pqo0sP2EJL8IbmueM6TXhKEpWu-_amunCq9n5wNclfHxPYZ0GhEWoKqE3MAnAI8erj7nng8-PPiUya6Y3x66iI09VCOM7AjL_RkZsYP9PGtszH726we_GH9-lUkopXFhvJwNtPtsNZ0MwrKW1yd4gWE_9GeAsP-5ALuOLoQwk1SWm-yeb0o2Kh0fYpRd89fQQ", IsPrimary = true },
                new WebsiteTour.Models.Entities.TourImage { TourId = tour2.Id, ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuDsuAOAmyHthVIPgSMFE-vu1vf667YpKH2BzbR0xK1AJP7T1U358VvO7gZN3tCeo7rU_N7WKz8L4GTCT4l814TcIWhoTfpC3xjdt9Q2FSQaTHFzZ7Sv1bcquMmGBaYMqaARJjRt5Wie2JtWgaOXYJUy47jPP1MY9xO_JQahe4_HKdNHkpg2ZLJuxkhWwSbdmISO4rxBtS-MLQ2slYiopi7fuzMS2mbhQbWaGXqzpTZDPzaBgWvgqoX81fgtPEbYj4ZRBVzI6-YvCQ", IsPrimary = true }
            );
            context.SaveChanges();
        }

        if (!context.Itineraries.Any() && context.Tours.Any())
        {
            var firstTour = context.Tours.First();
            context.Itineraries.AddRange(
                new WebsiteTour.Models.Entities.Itinerary { TourId = firstTour.Id, DayNumber = 1, Title = "Hà Nội - Vịnh Hạ Long", Description = "Sáng: Xe đón quý khách tại Hà Nội đi Hạ Long.\nTrưa: Lên du thuyền, nhận phòng và thưởng thức hải sản.\nChiều: Khám phá hang Sửng Sốt, chèo thuyền Kayak.\nTối: Tiệc BBQ trên boong tàu, ngắm hoàng hôn và câu mực đêm." },
                new WebsiteTour.Models.Entities.Itinerary { TourId = firstTour.Id, DayNumber = 2, Title = "Đảo Ti Tốp - Hà Nội", Description = "Sáng: Tập Thái Cực Quyền đón bình minh. Thăm đảo Ti Tốp, tắm biển hoặc leo núi ngắm toàn cảnh Vịnh.\nTrưa: Trả phòng, thưởng thức bữa trưa sớm trên du thuyền.\nChiều: Tàu cập bến, xe đưa quý khách về Hà Nội. Kết thúc hành trình." }
            );
            context.SaveChanges();
        }
        if (!context.PromoCodes.Any())
        {
            context.PromoCodes.AddRange(
                new WebsiteTour.Models.Entities.PromoCode { Code = "SUMMER10", Title = "Giảm 10$ Cho Nhóm 4", Description = "Áp dụng cho đơn từ 500$", Category = "Du Lịch Mùa Hè", DiscountAmount = 10, MinOrderValue = 500, ValidUntil = DateTime.Now.AddMonths(1) },
                new WebsiteTour.Models.Entities.PromoCode { Code = "WELCOME50", Title = "Giảm 50$ Cho Tour Đầu Tiên", Description = "Áp dụng cho đơn từ 200$", Category = "Khách Hàng Mới", DiscountAmount = 50, MinOrderValue = 200, ValidUntil = DateTime.Now.AddMonths(1) },
                new WebsiteTour.Models.Entities.PromoCode { Code = "FREERIDE", Title = "Giảm 20$ Di Chuyển", Description = "Áp dụng cho mọi đơn hàng", Category = "Di Chuyển", DiscountAmount = 20, MinOrderValue = 0, ValidUntil = DateTime.Now.AddMonths(1) }
            );
            context.SaveChanges();
        }
        if (!context.Deals.Any() && context.Tours.Any())
        {
            var firstTour = context.Tours.First();
            context.Deals.Add(new WebsiteTour.Models.Entities.Deal 
            { 
                Title = $"Khám Phá Di Sản {firstTour.Title} – Ưu Đãi 40%", 
                DiscountPercentage = 40, 
                ValidUntil = DateTime.Now.AddDays(2).AddHours(14).AddMinutes(36), 
                TourId = firstTour.Id,
                ImageUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuBswWWkqlCsiQoK3ACgn27fFNoHsqRUT827n4V4EOK8q4g_dPXWlneWk4LyULS4FZYcphEeha3hYX5zuqVtp-eYB2dx3zJ5wjbn4GqQJsVAKhlgUz39XElR0n7Qb8P4BXx0vKmLjumFtRrssKsLBb3L4vlRi4Jl9fyRJZ2JtWD9n3ZAz2lX9VR5sGxwV6B2cjWDpBv65Wg_ulACHUuuTTSE1rl2X_hxEjmQSZmm1DzIUW9Dpf6GyKdbhwPE6UB97r-JEEOrParMSw"
            });
            context.SaveChanges();
        }
        if (!context.TourSchedules.Any() && context.Tours.Any())
        {
            var tours = context.Tours.ToList();
            foreach (var tour in tours)
            {
                context.TourSchedules.AddRange(
                    new WebsiteTour.Models.Entities.TourSchedule { TourId = tour.Id, StartDate = DateTime.Now.AddDays(7), TotalSeats = 20, AvailableSeats = 15 },
                    new WebsiteTour.Models.Entities.TourSchedule { TourId = tour.Id, StartDate = DateTime.Now.AddDays(14), TotalSeats = 20, AvailableSeats = 2 },
                    new WebsiteTour.Models.Entities.TourSchedule { TourId = tour.Id, StartDate = DateTime.Now.AddDays(21), TotalSeats = 20, AvailableSeats = 20 }
                );
            }
            context.SaveChanges();
        }
        
        // Ensure Roles exist
        var roles = new[] { "Admin", "Customer" };
        foreach (var roleName in roles)
        {
            if (!context.Roles.Any(r => r.Name == roleName))
            {
                Console.WriteLine($"--> Seeding Role: {roleName}...");
                context.Roles.Add(new WebsiteTour.Models.Entities.Role { Name = roleName });
            }
        }
        context.SaveChanges();
        
        // Ensure Admin user exists with correct credentials
        var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (adminRole != null)
        {
            var adminUser = context.Users.FirstOrDefault(u => u.Username == "admin");
            if (adminUser == null)
            {
                Console.WriteLine("--> Creating Admin user...");
                context.Users.Add(new WebsiteTour.Models.Entities.User
                {
                    Username = "admin",
                    PasswordHash = "admin123",
                    Email = "admin@vietexplore.com",
                    RoleId = adminRole.Id
                });
            }
            else
            {
                Console.WriteLine("--> Updating Admin user password and role...");
                adminUser.PasswordHash = "admin123";
                adminUser.RoleId = adminRole.Id;
            }
            context.SaveChanges();
            Console.WriteLine("--> Admin user is ready (admin / admin123)");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Could not migrate or seed database: " + ex.Message);
    }
}

app.Run();
