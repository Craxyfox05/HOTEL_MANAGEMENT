using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // get user bookings
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.UserId == user.Id)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            ViewBag.Room = room;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int roomId, DateTime checkIn, DateTime checkOut)
        {
            // validate dates
            if (checkIn >= checkOut)
            {
                ModelState.AddModelError("", "Check-out date must be after check-in date.");
                var room = await _context.Rooms.FindAsync(roomId);
                ViewBag.Room = room;
                return View();
            }

            if (checkIn < DateTime.Today)
            {
                ModelState.AddModelError("", "Check-in date cannot be in the past.");
                var room = await _context.Rooms.FindAsync(roomId);
                ViewBag.Room = room;
                return View();
            }

            // check room availability
            var conflictingBooking = await _context.Bookings
                .AnyAsync(b => b.RoomId == roomId &&
                              b.CheckIn < checkOut &&
                              b.CheckOut > checkIn &&
                              b.PaymentStatus == "Completed");

            if (conflictingBooking)
            {
                ModelState.AddModelError("", "Room is not available for the selected dates.");
                var roomModel = await _context.Rooms.FindAsync(roomId);
                ViewBag.Room = roomModel;
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var roomCheck = await _context.Rooms.FindAsync(roomId);
            if (roomCheck == null) return NotFound();

            // calculate total
            var days = (checkOut - checkIn).Days;
            var totalAmount = roomCheck.Price * days;

            var booking = new Booking
            {
                UserId = user.Id,
                RoomId = roomId,
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalAmount = totalAmount,
                PaymentStatus = "Pending",
                BookingDate = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["BookingId"] = booking.Id;
            return RedirectToAction("Payment", new { bookingId = booking.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Payment(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (booking.UserId != user!.Id)
            {
                return Forbid();
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (booking.UserId != user!.Id)
            {
                return Forbid();
            }

            booking.PaymentStatus = "Completed";
            booking.Room.Status = "Booked";

            var payment = new Payment
            {
                BookingId = bookingId,
                Method = "Online",
                Amount = booking.TotalAmount,
                Date = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking confirmed successfully!";
            return RedirectToAction("Details", new { id = bookingId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (booking.UserId != user!.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (booking.UserId != user!.Id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            // Allow cancellation if check-in is at least 1 day away
            if (booking.CheckIn <= DateTime.Today.AddDays(1))
            {
                TempData["Error"] = "Cannot cancel booking. Check-in is within 24 hours.";
                return RedirectToAction("Details", new { id });
            }

            booking.Room.Status = "Available";
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Booking cancelled successfully!";
            return RedirectToAction("Index");
        }
    }
}

