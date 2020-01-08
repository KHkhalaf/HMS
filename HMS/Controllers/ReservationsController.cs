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
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> userMng;

        public ReservationsController(ApplicationDbContext context, UserManager<Account> userManager)
        {
            _context = context;
            userMng = userManager;
        }

        // GET: Reservations
        // return view contains all reservations that had reservated by customer
        // written By khalil Email: bestmind11111@gmail.com
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Index()
        {
            var result = await _context.Set<Reservation>().Include(
                reservation => reservation.Customer).Include(
                reservation => reservation.Room).ToListAsync();
            return View(result);
        }

        // GET: Reservations/returnReservationForCustomer
        // return Reservations had reservated For Customer
        // written By khalil Email: bestmind11111@gmail.com
        public async Task<IActionResult> returnReservationForCustomer()
        {
            var currentUser = await userMng.GetUserAsync(HttpContext.User);
            var result = await _context.Set<Reservation>().Include(
                reservation => reservation.Customer).Include(
                reservation => reservation.Room).Where(r => r.Customer.Id == currentUser.Id).ToListAsync();
            return View(result);
        }

        // GET: Reservations/Details/5
        // written By khalil Email: bestmind11111@gmail.com
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Set<Reservation>().Include(
                reservation => reservation.Customer).Include(
                reservation => reservation.Room).FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        // return view for create a reservation room
        // written By khalil Email: bestmind11111@gmail.com
        [Authorize(Roles = "Admin, Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // get the inputs from view for create a reservation room
        // written By khalil Email: bestmind11111@gmail.com
        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,DateIn,DateOut,Customer_id,Room_id")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Set<Reservation>().Include(
                reservation => reservation.Customer).Include(
                reservation => reservation.Room).FirstOrDefaultAsync(m => m.Id == id); ;
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // get the inputs from view for Edit a reservation room
        // written By khalil Email: bestmind11111@gmail.com
        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,DateIn,DateOut,Customer_id,Room_id")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        // return view for delete a reservation by id
        // written By khalil Email: bestmind11111@gmail.com
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Set<Reservation>().Include(
                reservation => reservation.Customer).Include(
                reservation => reservation.Room).FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        // delete a reservation by id
        // written By khalil Email: bestmind11111@gmail.com
        [Authorize(Roles = "Admin, Staff")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
