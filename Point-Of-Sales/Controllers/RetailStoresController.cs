using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Point_Of_Sales.Config;
using Point_Of_Sales.Entities;

namespace Point_Of_Sales.Controllers
{
    public class RetailStoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RetailStoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RetailStores
        public async Task<IActionResult> Index()
        {
            return _context.RetailStores != null ?
                        View(await _context.RetailStores.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.RetailStores'  is null.");
        }

        // GET: RetailStores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RetailStores == null)
            {
                return NotFound();
            }

            var retailStore = await _context.RetailStores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (retailStore == null)
            {
                return NotFound();
            }

            return View(retailStore);
        }

        [HttpGet]
        public async Task<IActionResult> Report([FromQuery]int q)
        {
            var id = User.FindFirst("Id")?.Value;
            if (id == null) return NotFound();

            var orders = new List<Purchase>();
            
            if (q != 0)
            {
                orders = _context.PurchaseHistories.Where(or => or.Employee.RetailStoreId == q && or.Received_Money >= or.Total_Amount).ToList();
            }
            else
            {
                var retailId = _context.Accounts.FirstOrDefault(p => p.Id == Convert.ToInt32(id))?.Employee?.RetailStoreId;
                
                if(retailId != null)
                {
                    orders = _context.PurchaseHistories.Where(or => or.Employee.RetailStoreId == retailId && or.Received_Money >= or.Total_Amount).ToList();
                }

                if (User.IsInRole("Head"))
                {
                    orders = _context.PurchaseHistories.Where(or => or.Received_Money >= or.Total_Amount).ToList();

                }
            }

            ViewBag.TotalOrder = orders.Count;

            var totalAmount = orders.Sum(or => or.Total_Amount);
            double cost = 0;

            foreach (var or in orders)
            {
                cost += or.PurchaseDetails.Sum(p => p.Product.Import_Price * p.Quantity);
            }

            ViewBag.Orders = orders ?? new List<Purchase>();
            ViewBag.TotalAmount = totalAmount;
            ViewBag.Profit = totalAmount - cost;

            return View();
        }

        // GET: RetailStores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RetailStores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RetailStoreID,Name,Address")] RetailStore retailStore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(retailStore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(retailStore);
        }

        // GET: RetailStores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RetailStores == null)
            {
                return NotFound();
            }

            var retailStore = await _context.RetailStores.FindAsync(id);
            if (retailStore == null)
            {
                return NotFound();
            }
            return View(retailStore);
        }

        // POST: RetailStores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RetailStoreID,Name,Address")] RetailStore retailStore)
        {
            if (id != retailStore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(retailStore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetailStoreExists(retailStore.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(retailStore);
        }

        // GET: RetailStores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RetailStores == null)
            {
                return NotFound();
            }

            var retailStore = await _context.RetailStores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (retailStore == null)
            {
                return NotFound();
            }

            return View(retailStore);
        }

        // POST: RetailStores/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RetailStores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RetailStores'  is null.");
            }
            var retailStore = await _context.RetailStores.FindAsync(id);
            if (retailStore != null)
            {
                _context.RetailStores.Remove(retailStore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RetailStoreExists(int id)
        {
            return (_context.RetailStores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
