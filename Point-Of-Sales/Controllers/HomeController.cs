using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Point_Of_Sales.Config;
using Point_Of_Sales.Entities;
using Point_Of_Sales.Models;
using System.Diagnostics;

namespace Point_Of_Sales.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Index()
        {
            var id = User.FindFirst("Id")?.Value;
            var listProducts = new List<Product>();
            if (id != null)
            {
                var retailId = _context.Accounts.FirstOrDefault(p => p.Id == System.Convert.ToInt32(id))?.Employee?.RetailStoreId;
                if (retailId != null)
                {
                    listProducts = _context.Products.Where(p => p.Inventories.Any(inv => inv.RetailStoreId == retailId)).ToList();
                }
            }

            return View(listProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}