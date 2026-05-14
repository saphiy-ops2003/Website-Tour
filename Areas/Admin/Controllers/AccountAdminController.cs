using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace WebsiteTour.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountAdminController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            if (username == "admin" && password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");

                await HttpContext.SignInAsync("CookieAuth",
                    new ClaimsPrincipal(identity));

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "DashboardAdmin", new { area = "Admin" });
            }

            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login", "AccountAdmin", new { area = "Admin" });
        }
    }
}