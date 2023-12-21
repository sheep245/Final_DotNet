﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Point_Of_Sales.Config;
using System.Security.Claims;

namespace Point_Of_Sales.Controllers
{
    public class AuthController : Controller
    {
        private ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            return View("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] string username, [FromForm] string password)
        {
            if (User.Identity.IsAuthenticated)
            {
               return RedirectToAction("Index", "Home");
            }

            var _account = _context.Accounts.FirstOrDefault(a => a.Username.Equals(username) && a.Pwd.Equals(password));

            if (_account == null)
            {
                TempData["Message"] = "Please check your credentials.";
                return RedirectToAction("Index");
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Role, _account.Role),
            };

            if (_account.Employee != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, _account.Employee.Fullname));
                claims.Add(new Claim(ClaimTypes.Email, _account.Employee.Email));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Set to true for a persistent cookie
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1)) // Adjust the expiration time as needed
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");   
        }
    }
}
