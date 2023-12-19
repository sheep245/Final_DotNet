using Microsoft.AspNetCore.Mvc;
using Point_Of_Sales.Config;

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
        public async Task<IActionResult> Index([FromForm]string username, [FromForm] string password)
        {
            var _account = _context.Accounts.FirstOrDefault(a => a.Username.Equals(username) && a.Pwd.Equals(password));

            if (_account == null)
            {
                TempData["Message"] = "Please check your credentials.";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }


    }
}
