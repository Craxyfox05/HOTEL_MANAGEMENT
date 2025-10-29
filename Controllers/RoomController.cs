using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? type, decimal? minPrice, decimal? maxPrice, string? status)
        {
            var rooms = _context.Rooms.AsQueryable();

            // filter by type
            if (!string.IsNullOrEmpty(type))
            {
                rooms = rooms.Where(r => r.Type == type);
            }

            // filter by min price
            if (minPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price >= minPrice.Value);
            }

            // filter by max price
            if (maxPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                rooms = rooms.Where(r => r.Status == status);
            }

            // get distinct types for dropdown
            ViewBag.Types = await _context.Rooms.Select(r => r.Type).Distinct().ToListAsync();
            ViewBag.Statuses = new List<string> { "Available", "Booked", "Cleaning" };

            return View(await rooms.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
    }
}

