using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Point_Of_Sales.Config;
using Point_Of_Sales.Entities;
using Point_Of_Sales.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Point_Of_Sales.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
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

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var products = await _context.Products.Where(p => p.Barcode.Equals(q) || p.Product_Name.Contains(q)).ToListAsync();
            return Ok(new { code = 0, products = products });
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var id = User.FindFirst("Id")?.Value;
            if (id != null)
            {
                var retailId = _context.Accounts.FirstOrDefault(p => p.Id == System.Convert.ToInt32(id))?.Employee?.RetailStoreId;
                if (retailId != null)
                {
                    var retail = _context.RetailStores.FirstOrDefault(rt => rt.Id == retailId);
                    ViewBag.Retail = retail;
                }
            }

            ViewBag.Stores = _context.RetailStores.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Barcode,Product_Name,Import_Price,Retail_Price,Category,Creation_Date")] Product product, [FromForm] int storeId, IFormFile image)
        {
            product.Is_Deleted = true;
            product.Creation_Date = DateTime.Now;

            if (image != null)
            {
                var fileName = $"product-{product.Id}.png";
                var stringPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images", "products", fileName);

                FileProcess.FileUpload(image, stringPath);
                product.ImagePath = $"/images/products/{fileName}";
            }

            var retail = _context.RetailStores.FirstOrDefault(p => p.Id == storeId);

            var inventoryExist = _context.Inventories.FirstOrDefault(inv => inv.RetailStoreId == retail.Id && inv.Product.Barcode.Equals(product.Barcode));

            if (inventoryExist != null)
            {
                inventoryExist.Number = inventoryExist.Number + product.Quantity;
            }
            else
            {
                _context.Inventories.Add(new Inventory() {InventoryID = retail.RetailStoreID, Product = product, RetailStore = retail, Number = product.Quantity });
            }

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Barcode,Product_Name,Import_Price,Retail_Price,Category,Creation_Date,Is_Deleted")] Product product, [FromForm] int storeId, IFormFile image)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            try
            {
                // var store = await _context.RetailStores.FirstOrDefaultAsync(rt => rt.Id == storeId);

                if (image != null)
                {
                    var fileName = $"product-{product.Id}.png";
                    var stringPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images", "products", fileName);

                    FileProcess.FileUpload(image, stringPath);
                    product.ImagePath = $"/images/products/{fileName}";
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", new { id = id });
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
