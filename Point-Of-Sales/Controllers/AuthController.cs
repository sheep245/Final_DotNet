using Microsoft.AspNetCore.Mvc;
using Point_Of_Sales.Config;

namespace Point_Of_Sales.Controllers
{
    public class AuthController : Controller
    {
        private ApplicationDbContext context;
        public IActionResult Index()
        {
            return View("Login");
        }

        public async Task<IActionResult> Index(string username, string password)
        {
            var _account = context.Accounts.FirstOrDefault(a => a.Username.Equals(username) && a.Pwd.Equals(password));

            if (_account == null)
            {
                ViewBag.Message = "Please check your credentials.";
                return View("Login");
            }
            return RedirectToAction("Index", "Home");
        }


    }
}
