using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            // get statistics
            var totalRooms = await _context.Rooms.CountAsync();
            var bookedRooms = await _context.Rooms.CountAsync(r => r.Status == "Booked");
            var availableRooms = await _context.Rooms.CountAsync(r => r.Status == "Available");
            var totalRevenue = await _context.Payments.SumAsync(p => p.Amount);

            ViewBag.TotalRooms = totalRooms;
            ViewBag.BookedRooms = bookedRooms;
            ViewBag.AvailableRooms = availableRooms;
            ViewBag.TotalRevenue = totalRevenue;

            // booking stats for chart
            var monthlyBookings = await _context.Bookings
                .Where(b => b.BookingDate.Year == DateTime.Now.Year)
                .GroupBy(b => b.BookingDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            ViewBag.MonthlyBookings = monthlyBookings;

            // recent bookings
            var recentBookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .OrderByDescending(b => b.BookingDate)
                .Take(10)
                .ToListAsync();

            ViewBag.RecentBookings = recentBookings;

            return View();
        }

        // Room Management
        public async Task<IActionResult> Rooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult CreateRoom()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Room created successfully!";
                return RedirectToAction("Rooms");
            }
            return View(room);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoom(int id, Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Room updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction("Rooms");
            }
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Room deleted successfully!";
            }
            return RedirectToAction("Rooms");
        }

        // Customer Management
        public async Task<IActionResult> Customers()
        {
            var customers = await _userManager.GetUsersInRoleAsync("User");
            return View(customers);
        }

        // Booking Management
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Include(b => b.Payment)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        // Reports
        public async Task<IActionResult> Reports(string? period)
        {
            IQueryable<Booking> bookingsQuery = _context.Bookings.Include(b => b.Room).Include(b => b.User);

            if (period == "daily")
            {
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate.Date == DateTime.Today);
            }
            else if (period == "monthly")
            {
                bookingsQuery = bookingsQuery.Where(b => b.BookingDate.Month == DateTime.Now.Month && b.BookingDate.Year == DateTime.Now.Year);
            }

            var bookings = await bookingsQuery.ToListAsync();
            ViewBag.Period = period ?? "all";
            return View(bookings);
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
