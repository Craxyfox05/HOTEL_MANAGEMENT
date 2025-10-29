namespace HotelManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Method { get; set; } = string.Empty; // Payment method (e.g., Online)
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        
        public Booking Booking { get; set; } = null!;
    }
}

