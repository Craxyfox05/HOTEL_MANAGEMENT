using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Completed
        public DateTime BookingDate { get; set; } = DateTime.Now;
        
        public ApplicationUser User { get; set; } = null!;
        public Room Room { get; set; } = null!;
        public Payment? Payment { get; set; }
    }
}

