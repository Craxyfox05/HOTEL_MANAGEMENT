namespace HotelManagementSystem.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public ApplicationUser User { get; set; } = null!;
    }
}

