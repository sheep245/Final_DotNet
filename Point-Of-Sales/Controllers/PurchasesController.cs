using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Point_Of_Sales.Config;
using Point_Of_Sales.Entities;
using Point_Of_Sales.Models;

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
            var purchases = await _context.PurchaseHistories.ToListAsync();
            return View(purchases);
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.PurchaseHistories == null)
            {
                return NotFound();
            }

            var purchase = await _context.PurchaseHistories.FirstOrDefaultAsync(m => m.purchaseId.Equals(id));
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        //public IActionResult Create()
        //{
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
        //    ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseModel order)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Phone.Equals(order.Customer.Phone));
            var employee = _context.Employees.FirstOrDefault(e => e.Id == order.EmployeeId);

            if (customer == null)
            {
                customer = new Customer()
                {
                    Phone = order.Customer.Phone,
                    Name = order.Customer.Name,
                    Address = order.Customer.Address,
                };
                _context.Customers.Add(customer);
            }

            if (employee == null)
            {
                ViewBag.Messsage = "Khong tim thay thang nhan vien nay";
                return View();
            }

            var purchase = new Purchase()
            {
                Customer = customer,
                purchaseId = Guid.NewGuid().ToString(),
                Employee = employee,
                Date_Of_Purchase = DateTime.Now
            };

            foreach (var detail in order.Products)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == detail.Id);

                var pdetail = new PurchaseDetail()
                {
                    Purchase = purchase,
                    Product = product,
                    Subtotal = detail.Subtotal,
                    Quantity = detail.Quantity,
                };

                _context.PurchaseDetails.Add(pdetail);
                purchase.PurchaseDetails.Add(pdetail);
            }

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(new { code = 0, returnUrl = "/Purchases/Checkout/" + purchase.purchaseId });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(string id)
        {
            var purchase = await _context.PurchaseHistories.FirstOrDefaultAsync(p => p.purchaseId.Equals(id));
            return View(purchase);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string id, [FromForm] double receivedMoney)
        {
            var purchase = await _context.PurchaseHistories.FirstOrDefaultAsync(p => p.purchaseId.Equals(id));

            if (purchase == null) return NotFound();


            purchase.Received_Money = receivedMoney;
            purchase.Paid_Back = receivedMoney - purchase.Total_Amount;

            await _context.SaveChangesAsync();

            ViewBag.Message = "Thanh toan thanh cong rui ne 🥰 Xin chan thanh cam on quy khach! Hen gap lai lan sau nhaaaaaa!";
            return View();
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
