using Microsoft.AspNetCore.Authentication;
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

            if (!_account.Employee.Status)
            {
                TempData["Message"] = "You must log in by the link sent via your Email.";
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
            claims.Add(new Claim("Id", _account.Id.ToString()));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Set to true for a persistent cookie
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1)) // Adjust the expiration time as needed
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

            if (_account.IsFirstLogin == true)
            {
                _account.IsFirstLogin = false;
                _context.SaveChanges();
                return RedirectToAction("ChangePassword", "Accounts", new { id = _account.Id });
            }

            if (_account.Role.Equals("Head"))
            {
                return RedirectToAction("Index", "RetailStores");
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Verify([FromQuery] string username, [FromQuery] string token, [FromQuery] string expire)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Username.Equals(username));

            if (account == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (Helpers.HelperConfirm.VerifyLink(username, token, expire))
            {
                account.Employee.Status = true;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return Content("The login link has expired! Contact the admin to resend it.");
        }
    }
}