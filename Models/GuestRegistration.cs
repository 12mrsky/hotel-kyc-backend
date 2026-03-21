using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_KYC_Api.Models
{
    public class GuestRegistration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public virtual HotelRegistration? Hotel { get; set; }

        [Required]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        public string GuestName { get; set; } = string.Empty;

        public string CheckInTime { get; set; } = string.Empty;
        public string CheckOutTime { get; set; } = string.Empty;

        public int Adults { get; set; } = 1;
        public int Kids { get; set; } = 0;

        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar must be 12 digits.")]
        public string AadhaarNumber { get; set; } = string.Empty;

        public int Age { get; set; }

        [Required]
        public string MobileNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string ComingFrom { get; set; } = string.Empty;
        public string GoingTo { get; set; } = string.Empty;

        // ✅ FIXED: UTC for PostgreSQL
        public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

        public bool IsFlagged { get; set; } = false;

        public string PoliceRemarks { get; set; } = string.Empty;

        public string Status { get; set; } = "Checked-In";
    }
}