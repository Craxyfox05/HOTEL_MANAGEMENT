using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Data;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // create database if not exists
            await context.Database.MigrateAsync();

            // create roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // create admin user
            if (await userManager.FindByEmailAsync("admin@hotel.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@hotel.com",
                    Email = "admin@hotel.com",
                    EmailConfirmed = true,
                    Name = "Admin User",
                    Phone = "1234567890"
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // seed rooms data
            if (!context.Rooms.Any())
            {
                var rooms = new List<Room>
                {
                    new Room { RoomNo = "101", Type = "Single", Price = 1500, Status = "Available", Description = "Comfortable single room with modern amenities", ImageURL = "/images/room1.jpg" },
                    new Room { RoomNo = "102", Type = "Single", Price = 1500, Status = "Available", Description = "Comfortable single room with modern amenities", ImageURL = "/images/room1.jpg" },
                    new Room { RoomNo = "201", Type = "Double", Price = 2500, Status = "Available", Description = "Spacious double room perfect for couples", ImageURL = "/images/room2.jpg" },
                    new Room { RoomNo = "202", Type = "Double", Price = 2500, Status = "Available", Description = "Spacious double room perfect for couples", ImageURL = "/images/room2.jpg" },
                    new Room { RoomNo = "301", Type = "Deluxe", Price = 4000, Status = "Available", Description = "Luxurious deluxe room with premium facilities", ImageURL = "/images/room3.jpg" },
                    new Room { RoomNo = "302", Type = "Deluxe", Price = 4000, Status = "Available", Description = "Luxurious deluxe room with premium facilities", ImageURL = "/images/room3.jpg" },
                    new Room { RoomNo = "401", Type = "Suite", Price = 6000, Status = "Available", Description = "Ultra-luxurious suite with separate living area", ImageURL = "/images/room4.jpg" },
                    new Room { RoomNo = "402", Type = "Suite", Price = 6000, Status = "Available", Description = "Ultra-luxurious suite with separate living area", ImageURL = "/images/room4.jpg" }
                };

                context.Rooms.AddRange(rooms);
                await context.SaveChangesAsync();
            }

            // seed staff data
            if (!context.Staff.Any())
            {
                var staff = new List<Staff>
                {
                    new Staff { Name = "John Smith", Role = "Manager", Contact = "john.smith@hotel.com", ShiftTime = "9:00 AM - 5:00 PM" },
                    new Staff { Name = "Sarah Johnson", Role = "Receptionist", Contact = "sarah.j@hotel.com", ShiftTime = "8:00 AM - 4:00 PM" },
                    new Staff { Name = "Mike Wilson", Role = "Housekeeping", Contact = "mike.w@hotel.com", ShiftTime = "6:00 AM - 2:00 PM" },
                    new Staff { Name = "Emily Davis", Role = "Concierge", Contact = "emily.d@hotel.com", ShiftTime = "10:00 AM - 6:00 PM" }
                };

                context.Staff.AddRange(staff);
                await context.SaveChangesAsync();
            }
        }
    }
}
