using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Point_Of_Sales.Config;
using Point_Of_Sales.Entities;
using Point_Of_Sales.Helpers;
using Point_Of_Sales.Models;

namespace Point_Of_Sales.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;

        public AccountsController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Accounts.ToList();
            return View(applicationDbContext);
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            ViewBag.Stores = _context.RetailStores.ToList();
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == id);

            account.Employee.Status = !account.Employee.Status;

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new { code = 0, status = account.Employee.Status, Message = "Change Status Successfully!" });
            }

            return Ok(new { code = 1, Message = "Change Status Failed!" });

        }


        public async Task<IActionResult> Resend(string username)
        {
            return Ok();
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            // ViewData["Stores"] = new SelectList(_context.RetailStores, "Id", "Name");
            ViewBag.Stores = _context.RetailStores.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountModel account)
        {
            if (ModelState.IsValid)
            {
                var username = account.Email.Split("@")[0].Trim();
                var pwd = username;

                var store = _context.RetailStores.FirstOrDefault(s => s.Id == account.RetailStoreId);

                var newAccount = new Account()
                {
                    Username = username,
                    Pwd = pwd,
                    Role = Helpers.Convert.Capitalize(account.Role),
                };

                var employee = new Employee()
                {
                    Account = newAccount,
                    Fullname = account.Fullname,
                    Email = account.Email,
                    RetailStore = store
                };

                _context.Employees.Add(employee);
                _context.Accounts.Add(newAccount);

                var result = await _context.SaveChangesAsync();

                // Create link verify
                if (result > 0)
                {
                    string subject = "";
                    var link = $"{Request.Scheme}://{Request.Host}" + Helpers.HelperConfirm.Generatelink(username);

                    string content = $"Welcome to my app. Please click <a href=\"{link}\"> here </a> to active your account.";

                    var mailer = new Mailer(_configuration);

                    mailer.SendEmail(account.Email, subject, content);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Stores = _context.RetailStores.ToList();
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", account.EmployeeId);
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] AccountModel newAccount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            account.Employee.Fullname = newAccount.Fullname;
            account.Role = newAccount.Role;
            account.Employee.RetailStoreId = newAccount.RetailStoreId;

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", new { id = id });

        }

        [HttpPost]
        public async Task<IActionResult> ReSend(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            string subject = "";
            var link = $"{Request.Scheme}://{Request.Host}" + Helpers.HelperConfirm.Generatelink(account.Username);

            string content = $"Welcome to my app. Please click <a href=\"{link}\"> here </a> to active your account.";

            var mailer = new Mailer(_configuration);

            mailer.SendEmail(account.Employee.Email, subject, content);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var profile = (await _context.Accounts.FindAsync(id));

            if (profile == null) return NotFound();
            
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(int id, [FromForm] string CurPwd, [FromForm] string NewPwd, [FromForm] string Confirm)
        {
            var account = (await _context.Accounts.FindAsync(id));

            if (account == null) return NotFound();

            if (!CurPwd.Equals(account.Pwd))
            {
                TempData["Message"] = "Your current password does not match.";
                return RedirectToAction("ChangePassword", new { id = id });
            }

            if (!NewPwd.Equals(Confirm))
            {
                TempData["Message"] = "Confirm new password is not correct.";
                return RedirectToAction("ChangePassword", new { id = id });
            }

            account.Pwd = NewPwd;
            await _context.SaveChangesAsync();
            TempData["Message"] = "Change password successfully.";

            return RedirectToAction("ChangePassword", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(int id)
        {
            var account = (await _context.Accounts.FindAsync(id));

            if (account == null) return NotFound();

            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery]string q)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Phone.Equals(q));
            if (customer == null) return Ok();
            return Ok(new {phone = customer.Phone, name = customer.Name, address = customer.Address});
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
