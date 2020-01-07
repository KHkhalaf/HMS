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
    public class AccountViewModelsController : Controller
    {
        private UserManager<AccountViewModel> UserMng { get; set; } 
        private SignInManager<AccountViewModel> SignMng { get; set; }
        private ApplicationDbContext _context { get; set; }
        public AccountViewModelsController(ApplicationDbContext applicationDbContext,
            UserManager<AccountViewModel> UserMng, SignInManager<AccountViewModel> SignMng)
        {
            this.UserMng = UserMng;
            this.SignMng = SignMng;
            this._context = applicationDbContext;
        }
        [Authorize(Roles ="Admin")]
        // GET: AccountViewModels for Staffs
        public IActionResult Index()
        {
            return View(GetUserByRole("Staff"));
        }

        [Authorize(Roles = "Admin, Staff")]
        // GET: AccountViewModels for Customers
        public IActionResult Customers()
        {
            return View(GetUserByRole("Customer"));
        }


        // GET: AccountViewModels/DisplayUserProfile/
        public async Task<IActionResult> DisplayUserProfile()
        {
            var accountViewModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == (UserMng.GetUserAsync(HttpContext.User).Result).Id);
            if (accountViewModel == null)
            {
                return NotFound();
            }

            return View(accountViewModel);
        }

        // GET: AccountViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountViewModel = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountViewModel == null)
            {
                return NotFound();
            }

            return View(accountViewModel);
        }

        [AllowAnonymous]
        // GET: AccountViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AccountViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountViewModel accountViewModel)
        {
            var account = await UserMng
                .FindByEmailAsync(accountViewModel.Email);
            if (ModelState.IsValid && account == null)
            {
                var result = await UserMng.CreateAsync(accountViewModel);
                var result1 = await UserMng.AddToRoleAsync(accountViewModel, "Staff");

                return RedirectToAction("Index", "Home");
            }
            ViewBag.EmailExist = "This Email is Exist ...";
            return View(accountViewModel);
        }

        // GET: AccountViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountViewModel = await UserMng.FindByIdAsync(id.ToString());
            if (accountViewModel == null)
            {
                return NotFound();
            }
            return View(accountViewModel);
        }

        // POST: AccountViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,AccountViewModel accountViewModel)
        {
            if (id != accountViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UserMng.UpdateAsync(accountViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountViewModelExists(accountViewModel.Id))
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
            return View(accountViewModel);
        }

        // GET: AccountViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountViewModel = await UserMng.FindByIdAsync(id.ToString());
            if (accountViewModel == null)
            {
                return NotFound();
            }

            return View(accountViewModel);
        }

        // POST: AccountViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountViewModel = await UserMng.FindByIdAsync(id.ToString());
            await UserMng.DeleteAsync(accountViewModel);
            return RedirectToAction(nameof(Index));
        }

        private bool AccountViewModelExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // GET: LoginViewModels/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: AccountViewModels/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountViewModel accountViewModel)
        {
            var account = await UserMng
                .FindByEmailAsync(accountViewModel.Email);
            if (ModelState.IsValid && account == null)
            {
                var result = await UserMng.CreateAsync(accountViewModel);
                var result1 = await UserMng.AddToRoleAsync(accountViewModel, "Customer");

                if (result.Succeeded && result1.Succeeded)
                {
                    await SignMng.SignInAsync(accountViewModel, isPersistent: false);
                }
                return RedirectToAction("Index", "Home");
            }
            ViewBag.EmailExist = "This Email is Exist ...";
            return View(accountViewModel);
        }

        // GET: LoginViewModels/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: AccountViewModels/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountViewModel accountViewModel)
        {
            var account = await _context.Users
                .FirstOrDefaultAsync(m => m.Email == accountViewModel.Email);
            if (account != null && account.Password == accountViewModel.Password)
            {
                await SignMng.SignInAsync(account, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.InputValid = "Check Your Email or Password ...";
            return View(accountViewModel);
        }

        // Logout User
        public async Task<IActionResult> LogOut()
        {
            await SignMng.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private List<AccountViewModel> GetUserByRole(string RoleName)
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
            var _customers = new List<AccountViewModel>();
            foreach (var item in listStaffs.ToList())
            {
                var account = new AccountViewModel();
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
