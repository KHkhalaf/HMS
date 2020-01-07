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
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AccountViewModel> userMng;
        public ServicesController(ApplicationDbContext context,UserManager<AccountViewModel> userManager)
        {
            _context = context;
            userMng = userManager;
        }

        // GET: Services
        [Authorize(Roles ="Admin, Staff")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Set<Service>()
                .Include(s => s.food)
                .Include(s => s.drink)
                .Include(s => s.User)
                .ToListAsync());
        }

        // GET: Services/CreateDrinkOrder
        public async Task<IActionResult> CreateDrinkOrder()
        {
            return View(await _context.Drinks.ToListAsync());
        }

        // POST: Services/AddDrink_returnOrdersById/4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDrinkOrder(int Id,string RoomNo)
        {
            var drink = _context.Drinks.FirstOrDefaultAsync(d => d.Id == Id);
            var service = new Service();
            if (ModelState.IsValid)
            {
                service.Service_Type = (new ServiceType().getType(1));
                service.Service_Date = DateTime.Now;
                service.Status = (new Order_Status().getType(2));
                service.Cost = drink.Result.Price;
                service.drink = drink.Result;
                service.Room_NO = Int32.Parse(RoomNo);
                service.Description = "This is a one-time drink order";
                service.User = await userMng.GetUserAsync(HttpContext.User);
                _context.Add(service);
                await _context.SaveChangesAsync();
            }
            var currentUser = await userMng.GetUserAsync(HttpContext.User);
            return RedirectToAction(nameof(ReturnServicesForCustomers));
        }

        // GET: Services/CreateTableReservation
        public async Task<IActionResult> CreateFoodOrder()
        {
            return View(await _context.Foods.ToListAsync());
        }

        //// POST: Services/AddFood_returnOrdersById/4      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFoodOrder(int Id,string RoomNo)
        {
            var food = _context.Drinks.FirstOrDefaultAsync(f => f.Id == Id);
            var service = new Service();
            if (ModelState.IsValid)
            {
                service.Service_Type = (new ServiceType().getType(0));
                service.Service_Date = DateTime.Now;
                service.Status = (new Order_Status().getType(2));
                service.Cost = food.Result.Price;
                service.drink = food.Result;
                service.Room_NO = Int32.Parse(RoomNo);
                service.Description = "This is a one-time food order";
                service.User = await userMng.GetUserAsync(HttpContext.User);
                _context.Add(service);
                await _context.SaveChangesAsync();
            }
            var currentUser = await userMng.GetUserAsync(HttpContext.User);
            return RedirectToAction(nameof(ReturnServicesForCustomers));
        }

        public async Task<IActionResult> ReturnServicesForCustomers()
        {
            var currentUser = await userMng.GetUserAsync(HttpContext.User);
            return View(await _context.Set<Service>()
                .Include(s => s.food)
                .Include(s => s.drink)
                .Include(s => s.User).Where(s => s.User.Id == currentUser.Id)
                .ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Set<Service>()
                .Include(s => s.food)
                .Include(s => s.drink)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }


        public IActionResult CreateTableReservation()
        {
            return View();
        }

        // POST: Services/CreateTableReservation
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTableReservation(Service service)
        {
            if (ModelState.IsValid)
            {
                service.Service_Type = (new ServiceType().getType(4));
                service.Service_Date = DateTime.Now;
                service.Status = (new Order_Status().getType(2));
                service.Cost = 2000;
                service.User = await userMng.GetUserAsync(HttpContext.User);
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ReturnServicesForCustomers));
            }
            return View(service);
        }

        // GET: Services/CreateCleanClothes
        public IActionResult CreateCleanClothes()
        {
            return View();
        }

        // POST: Services/CreateCleanClothes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCleanClothes(Service service)
        {
            if (ModelState.IsValid)
            {
                service.Service_Type = (new ServiceType().getType(3));
                service.Service_Date = DateTime.Now;
                service.Status = (new Order_Status().getType(2));
                service.Cost = 100;
                service.User = await userMng.GetUserAsync(HttpContext.User);
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ReturnServicesForCustomers));
            }
            return View(service);
        }


        // GET: Services/CreateCleanRoom
        public IActionResult CreateCleanRoom()
        {
            return View();
        }

        // POST: Services/CreateCleanRoom
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCleanRoom(Service service)
        {
            if (ModelState.IsValid)
            {
                service.Service_Type = (new ServiceType().getType(2));
                service.Service_Date = DateTime.Now;
                service.Status = (new Order_Status().getType(2));
                service.Cost = 100;
                service.User = await userMng.GetUserAsync(HttpContext.User);
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ReturnServicesForCustomers));
            }
            return View(service);
        }

        // GET: Services/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Service = await _context.Services.FindAsync(id);
            if (Service == null)
            {
                return NotFound();
            }
            return View(Service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Service_Type,Room_NO,Status,Service_Date,Description,Cost,Table_No")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    service.Service_Date = DateTime.Now;
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
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
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
