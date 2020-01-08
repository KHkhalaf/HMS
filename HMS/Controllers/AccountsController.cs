using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HMS.Data;
using HMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private UserManager<Account> UserMng { get; set; } 
        private SignInManager<Account> SignMng { get; set; }
        private ApplicationDbContext _context { get; set; }
        public AccountsController(ApplicationDbContext applicationDbContext,
            UserManager<Account> UserMng, SignInManager<Account> SignMng)
        {
            this.UserMng = UserMng;
            this.SignMng = SignMng;
            this._context = applicationDbContext;
        }
        [Authorize(Roles ="Admin")]
        // GET: Accounts for Staffs
        // written By khalil Email: bestmind11111@gmail.com
        public IActionResult Index()
        {
            return View(GetUserByRole("Staff"));
        }

        [Authorize(Roles = "Admin, Staff")]
        // GET: Accounts for Customers
        // written By khalil Email: bestmind11111@gmail.com
        public IActionResult Customers()
        {
            return View(GetUserByRole("Customer"));
        }


        // GET: Accounts/DisplayUserProfile/
        // return view for display a profile
        // written By khalil Email: bestmind11111@gmail.com
        public async Task<IActionResult> DisplayUserProfile()
        {
            var Account = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == (UserMng.GetUserAsync(HttpContext.User).Result).Id);
            if (Account == null)
            {
                return NotFound();
            }

            return View(Account);
        }

        // GET: Accounts/Details/5
        // get details profile
        // written By khalil Email: bestmind11111@gmail.com 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Account = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Account == null)
            {
                return NotFound();
            }

            return View(Account);
        }

        [AllowAnonymous]
        // GET: Accounts/Create
        // get viwe for create (add) a staff by owner
        // written By khalil Email: bestmind11111@gmail.com 
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // get the inputs from view for create a account staff by owner
        // written By khalil Email: bestmind11111@gmail.com  
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account Account)
        {
            var account = await UserMng
                .FindByEmailAsync(Account.Email);
            if (ModelState.IsValid && account == null)
            {
                var result = await UserMng.CreateAsync(Account);
                var result1 = await UserMng.AddToRoleAsync(Account, "Staff");

                return RedirectToAction("Index", "Home");
            }
            ViewBag.EmailExist = "This Email is Exist ...";
            return View(Account);
        }

        // GET: Accounts/Edit/5
        // return view for edit a profile by user
        // written By khalil Email: bestmind11111@gmail.com
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Account = await UserMng.FindByIdAsync(id.ToString());
            if (Account == null)
            {
                return NotFound();
            }
            return View(Account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // get the inputs from view for update a account by user
        // written By khalil Email: bestmind11111@gmail.com 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UserMng.UpdateAsync(account);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        // return view for delete a account by owner
        // written By khalil Email: bestmind11111@gmail.com
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Account = await UserMng.FindByIdAsync(id.ToString());
            if (Account == null)
            {
                return NotFound();
            }

            return View(Account);
        }

        // POST: Accounts/Delete/5
        // get the inputs (id) for delete a account by owner
        // written By khalil Email: bestmind11111@gmail.com
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Account = await UserMng.FindByIdAsync(id.ToString());
            await UserMng.DeleteAsync(Account);
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // GET: Accounts/Register
        // return view for create a account customer 
        // written By khalil Email: bestmind11111@gmail.com
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Accounts/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // get the inputs from view for create a account customer 
        // written By khalil Email: bestmind11111@gmail.com
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Account Account)
        {
            var account = await UserMng
                .FindByEmailAsync(Account.Email);
            if (ModelState.IsValid && account == null)
            {
                var result = await UserMng.CreateAsync(Account);
                var result1 = await UserMng.AddToRoleAsync(Account, "Customer");

                if (result.Succeeded && result1.Succeeded)
                {
                    await SignMng.SignInAsync(Account, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.UserNameDublicated = "Your UserName is Dublicated ...";
                    return View(Account);
                }
            }
            ViewBag.EmailExist = "This Email is Exist ...";
            return View(Account);
        }
        [Authorize(Roles ="Customer")]
        // POST: Accounts/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // create an invoice for customer by {id}
        // written By khalil Email: bestmind11111@gmail.com
        public async Task<IActionResult> CreateInvoiceById(int Id)
        {
            var Account = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == (UserMng.GetUserAsync(HttpContext.User).Result).Id);
            var userRole = _context.UserRoles.Where(ur => ur.UserId == Account.Id);
            var role = _context.Roles.Where(r => r.Id == userRole.ToList()[0].RoleId);
            if (role.ToList()[0].Name == "Customer")
                Id = Account.Id;
            var invoice = new InvoiceViewModel();
            var Customer = await UserMng
                .FindByIdAsync(Id.ToString());
            var services = await _context.Set<Service>()
                .Include(s => s.food)
                .Include(s => s.drink)
                .Include(s => s.User).Where(s => s.User.Id == Customer.Id)
                .ToListAsync();
            var rooms = await _context.Set<Reservation>()
                .Include(r => r.Customer)
                .Include(r => r.Room).Where(r => r.Customer.Id == Customer.Id)
                .ToListAsync();
            int cost = 0;
            for(int i = 0; i < services.Count; i++)
            {
                cost += services[i].Cost;
            }
            invoice.Customer = Customer;
            invoice.Services = services;
            invoice.Rooms = rooms;
            invoice.Cost = cost;

            return View(invoice);
        }

        // GET: Accounts/Login
        // return view for login action
        // written By khalil Email: bestmind11111@gmail.com
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Accounts/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // get the inputs for login a user
        // written By khalil Email: bestmind11111@gmail.com
        [AllowAnonymous]
        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Account Account)
        {
            var account = await _context.Users
                .FirstOrDefaultAsync(m => m.Email == Account.Email);
            if (account != null && account.Password == Account.Password)
            {
                await SignMng.SignInAsync(account, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.InputValid = "Check Your Email or Password ...";
            return View(Account);
        }

        // return view for logout User
        // written By khalil Email: bestmind11111@gmail.com
        public async Task<IActionResult> LogOut()
        {
            await SignMng.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // get list of users (staffs , customers) by role
        // written By khalil Email: bestmind11111@gmail.com
        private List<Account> GetUserByRole(string RoleName)
        {
            var listStaffs = _context.Users.ToList()
                                .Join(_context.UserRoles.ToList(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                .Join(_context.Roles.ToList(), urr => urr.ur.RoleId, r => r.Id, (urr, r) => new { urr, r })
                                .Where(role => role.r.Name == RoleName)
                                .Select(m => new {
                                    userId = m.urr.u.Id,
                                    userName = m.urr.u.UserName,
                                    Email = m.urr.u.Email,
                                    phone = m.urr.u.PhoneNumber,
                                    city = m.urr.u.City,
                                    securitystamp = m.urr.u.SecurityStamp,
                                    normalizedUsername = m.urr.u.NormalizedUserName,
                                    normalizedEmail = m.urr.u.NormalizedEmail
                                });
            var _customers = new List<Account>();
            foreach (var item in listStaffs.ToList())
            {
                var account = new Account();
                account.Id = item.userId;
                account.UserName = item.userName;
                account.Email = item.Email;
                account.PhoneNumber = item.phone;
                account.City = item.city;
                _customers.Add(account);
            }
            return _customers;
        }
    }
}
