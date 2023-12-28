using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
            var id = User.FindFirst("Id")?.Value;
            if (id == null) return NotFound();

            var accounts = new List<Account>();

            var retailId = _context.Accounts.FirstOrDefault(p => p.Id == System.Convert.ToInt32(id))?.Employee?.RetailStoreId;

            if (retailId != null)
            {
                accounts = _context.Accounts.Where(ac => ac.Employee.RetailStoreId == retailId && !ac.Username.Equals("admin")).ToList();
            }

            if (User.IsInRole("Head"))
            {
                accounts = _context.Accounts.Where(ac => ac.Role.Equals("Admin")).ToList();
            }

            return View(accounts);
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
            ViewBag.Message = TempData["Message"] ?? null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountModel account)
        {
            ViewBag.Stores = _context.RetailStores.ToList();
            if (ModelState.IsValid)
            {
                var username = account.Email.Split("@")[0].Trim();
                var pwd = username;

                var exist = _context.Employees.FirstOrDefault(e => e.Email.Equals(account.Email));

                if (exist != null)
                {
                    TempData["Message"] = "Email already exists in the system.";
                    return RedirectToAction("Create");
                }

                var store = _context.RetailStores.FirstOrDefault(s => s.Id == account.RetailStoreId);

                var newAccount = new Account()
                {
                    Username = username,
                    Pwd = pwd,
                    Role = Helpers.Convert.Capitalize(account.Role),
                };
                _context.Accounts.Add(newAccount);
                _context.SaveChanges();


                var employee = new Employee()
                {
                    AccountId = newAccount.Id,  
                    Account = newAccount,
                    Fullname = account.Fullname,
                    Email = account.Email,
                    RetailStore = store,
                };

                newAccount.Employee = employee;

                _context.Employees.Add(employee);
                var result = _context.SaveChanges();


                // Create link verify
                if (result > 0)
                {
                    string subject = "";
                    var link = $"{Request.Scheme}://{Request.Host}" + Helpers.HelperConfirm.Generatelink(username);

                    string content = $"Welcome to my <b>Point Of Sales</b> website.<hr>Please click <a href=\"{link}\"> here</a> to active your account.";

                    var mailer = new Mailer(_configuration);

                    mailer.SendEmail(account.Email, subject, content);
                }

                return RedirectToAction(nameof(Index));
            }
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

            string content = $"Welcome to my <b>Point-Of-Sales</b> website.<br> Please click <a href=\"{link}\"> here</a> to active your account.";

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
        public async Task<IActionResult> Profile(int id, IFormFile image)
        {
            var profile = (await _context.Accounts.FindAsync(id));

            if (profile == null) return NotFound();

            if (image != null)
            {
                var fileName = $"profile-{profile.Id}.png";
                var stringPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images", "user", fileName);

                FileProcess.FileUpload(image, stringPath);
                profile.Employee.ImagePath = $"/images/user/{fileName}";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", new { id = id });
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

            if (account.IsFirstLogin)
            {
                account.IsFirstLogin = false;
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
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Phone.Equals(q));
            if (customer == null) return Ok();
            return Ok(new { phone = customer.Phone, name = customer.Name, address = customer.Address });
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
