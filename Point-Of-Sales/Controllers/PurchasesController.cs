using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Point_Of_Sales.Config;
using Point_Of_Sales.Entities;

namespace Point_Of_Sales.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PurchaseHistories.Include(p => p.Customer).Include(p => p.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchaseHistories == null)
            {
                return NotFound();
            }

            var purchase = await _context.PurchaseHistories
                .Include(p => p.Customer)
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("purchaseId,CustomerId,EmployeeId,Total_Amount,Received_Money,Paid_Back,Date_Of_Purchase")] Purchase purchase)
        {
            //var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == purchase.CustomerId);
            //var employee = await _context.Employees.FirstOrDefaultAsync(c => c.Id == purchase.EmployeeId);

            //purchase.Employee = employee;
            //purchase.Customer = customer;

            _context.Add(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", purchase.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", purchase.EmployeeId);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchaseHistories == null)
            {
                return NotFound();
            }

            var purchase = await _context.PurchaseHistories.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", purchase.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", purchase.EmployeeId);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,purchaseId,CustomerId,EmployeeId,Total_Amount,Received_Money,Paid_Back,Date_Of_Purchase")] Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", purchase.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", purchase.EmployeeId);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchaseHistories == null)
            {
                return NotFound();
            }

            var purchase = await _context.PurchaseHistories
                .Include(p => p.Customer)
                .Include(p => p.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchaseHistories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PurchaseHistories'  is null.");
            }
            var purchase = await _context.PurchaseHistories.FindAsync(id);
            if (purchase != null)
            {
                _context.PurchaseHistories.Remove(purchase);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
            return (_context.PurchaseHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
