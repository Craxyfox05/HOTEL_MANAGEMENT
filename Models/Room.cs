namespace HotelManagementSystem.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNo { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Single, Double, Deluxe, Suite
        public decimal Price { get; set; }
        public string Status { get; set; } = "Available"; // Available, Booked, Cleaning
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

