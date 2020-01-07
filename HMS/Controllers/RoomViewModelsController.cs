using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using HMS.Data;
using HMS.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HMS.Controllers
{
    [Authorize(Roles ="Admin, Staff")]
    public class RoomViewModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        public RoomViewModelsController(ApplicationDbContext context,IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: RoomViewModels
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.Where(
                status => status.Status == new Room_Status().getStatus(1)).ToListAsync());
        }

        // GET: RoomViewModels/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomViewModel = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomViewModel == null)
            {
                return NotFound();
            }

            return View(roomViewModel);
        }

        // GET: RoomViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomViewModel roomViewModel,IFormFile Fileimage)
        {
            if (ModelState.IsValid)
            {
                if (Fileimage != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    string uniqeFileName = Guid.NewGuid().ToString() + "_" + Fileimage.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqeFileName);
                    Fileimage.CopyTo(new FileStream(filePath, FileMode.Create));
                    roomViewModel.image = uniqeFileName;
                }
                _context.Add(roomViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomViewModel);
        }

        // GET: RoomViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomViewModel = await _context.Rooms.FindAsync(id);
            if (roomViewModel == null)
            {
                return NotFound();
            }
            return View(roomViewModel);
        }

        // POST: RoomViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomViewModel roomViewModel, IFormFile Fileimage, string imgName)
        {
            
            if (id != roomViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Fileimage != null)
                    {
                        string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        string uniqeFileName = Guid.NewGuid().ToString() + "_" + Fileimage.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqeFileName);
                        Fileimage.CopyTo(new FileStream(filePath, FileMode.Create));
                        roomViewModel.image = uniqeFileName;
                    }
                    else
                    {
                        roomViewModel.image = imgName;
                    }
                    _context.Update(roomViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomViewModelExists(roomViewModel.Id))
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
            return View(roomViewModel);
        }

        // GET: RoomViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomViewModel = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomViewModel == null)
            {
                return NotFound();
            }

            return View(roomViewModel);
        }

        // POST: RoomViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomViewModel = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(roomViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: RoomViewModels/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Reservation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomViewModel = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomViewModel == null)
            {
                return NotFound();
            }

            return View(roomViewModel);
        }
        // POST: RoomViewModels/Reservation/5
        [AllowAnonymous]
        [HttpPost,ActionName("Reservation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation(int id,string description, DateTime SelectedDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservation = new Reservation();
                reservation.Description = description;
                reservation.DateIn = DateTime.Now;
                reservation.DateOut = SelectedDate;
                reservation.Customer = await _context.Users.FirstOrDefaultAsync(u => u.Id == Int32.Parse(userId));
                reservation.Room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
                reservation.Customer_id = Int32.Parse(userId);
                reservation.Room_id = id;
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            room.Status = (new Room_Status()).getStatus(0);
            _context.Rooms.Update(room);
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomViewModelExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
