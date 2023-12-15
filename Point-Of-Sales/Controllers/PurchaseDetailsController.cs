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
    public class PurchaseDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PurchaseDetails.Include(p => p.Product).Include(p => p.Purchase);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PurchaseDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PurchaseDetails == null)
            {
                return NotFound();
            }

            var purchaseDetail = await _context.PurchaseDetails
                .Include(p => p.Product)
                .Include(p => p.Purchase)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseDetail == null)
            {
                return NotFound();
            }

            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["PurchaseId"] = new SelectList(_context.PurchaseHistories, "Id", "Id");
            return View();
        }

        // POST: PurchaseDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseId,ProductId,Quantity,Subtotal")] PurchaseDetail purchaseDetail)
        {
            _context.Add(purchaseDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", purchaseDetail.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.PurchaseHistories, "Id", "Id", purchaseDetail.PurchaseId);
            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PurchaseDetails == null)
            {
                return NotFound();
            }

            var purchaseDetail = await _context.PurchaseDetails.FindAsync(id);
            if (purchaseDetail == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", purchaseDetail.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.PurchaseHistories, "Id", "Id", purchaseDetail.PurchaseId);
            return View(purchaseDetail);
        }

        // POST: PurchaseDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PurchaseId,ProductId,Quantity,Subtotal")] PurchaseDetail purchaseDetail)
        {
            if (id != purchaseDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseDetailExists(purchaseDetail.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", purchaseDetail.ProductId);
            ViewData["PurchaseId"] = new SelectList(_context.PurchaseHistories, "Id", "Id", purchaseDetail.PurchaseId);
            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PurchaseDetails == null)
            {
                return NotFound();
            }

            var purchaseDetail = await _context.PurchaseDetails
                .Include(p => p.Product)
                .Include(p => p.Purchase)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseDetail == null)
            {
                return NotFound();
            }

            return View(purchaseDetail);
        }

        // POST: PurchaseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PurchaseDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PurchaseDetails'  is null.");
            }
            var purchaseDetail = await _context.PurchaseDetails.FindAsync(id);
            if (purchaseDetail != null)
            {
                _context.PurchaseDetails.Remove(purchaseDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseDetailExists(int id)
        {
          return (_context.PurchaseDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
