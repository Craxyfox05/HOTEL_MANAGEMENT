using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HotelManagementSystem.Models;
using HotelManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // get available rooms for homepage
            var rooms = await _context.Rooms
                .Where(r => r.Status == "Available")
                .Take(6)
                .ToListAsync();

            ViewBag.AvailableRooms = rooms;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

