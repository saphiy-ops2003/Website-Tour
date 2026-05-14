using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebsiteTour.Models;
using WebsiteTour.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebsiteTour.Services;
using WebsiteTour.Models.Entities;
using WebsiteTour.ViewModels;

namespace WebsiteTour.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AppDbContext Context => _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    // ================= LOGIN =================
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
            var user = await Context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.Email == model.EmailOrUsername ||
                    u.Username == model.EmailOrUsername);

            if (user != null && PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Customer")
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");

                await HttpContext.SignInAsync(
                    "CookieAuth",
                    new ClaimsPrincipal(identity),
                    new AuthenticationProperties { IsPersistent = true }
                );

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
        }

        return View(model);
    }

    // ================= REGISTER =================
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
            var exists = await Context.Users
                .AnyAsync(u => u.Email == model.Email || u.Username == model.Username);

            if (exists)
            {
                ModelState.AddModelError("", "Email hoặc Username đã tồn tại");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = PasswordHasher.HashPassword(model.Password)
            };

            Context.Users.Add(user);
            await Context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        return View(model);
    }

    // ================= LOGOUT =================
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login");
    }

    // ================= PROFILE =================
    [Authorize]
public async Task<IActionResult> Profile()
{
    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    var user = await Context.Users.FindAsync(userId);

    var model = new ProfileViewModel
    {
        Username = user.Username,
        Email = user.Email
    };

    return View(model);
}
}