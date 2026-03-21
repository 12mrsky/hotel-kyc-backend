using System.ComponentModel.DataAnnotations;

namespace Hotel_KYC_Api.Models
{
    public class HotelRegistration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HotelName { get; set; } = string.Empty;

        [Required]
        public string OwnerName { get; set; } = string.Empty;

        public string GSTNumber { get; set; } = string.Empty;

        [Required]
        [Phone] // ✅ validation added
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        // ✅ PostgreSQL SAFE
        public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    }
}