using Microsoft.AspNetCore.Identity;

namespace HotelManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? IdProof { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

