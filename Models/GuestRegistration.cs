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
        public string RoomNumber { get; set; }

        [Required]
        public string GuestName { get; set; }

        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }

        public int Adults { get; set; } = 1;
        public int Kids { get; set; } = 0;

        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar must be 12 digits.")]
        public string AadhaarNumber { get; set; }

        public int Age { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        public string Address { get; set; }
        public string ComingFrom { get; set; }
        public string GoingTo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsFlagged { get; set; } = false; // BIT maps to bool
        public string? PoliceRemarks { get; set; }
        public string Status { get; set; } = "Checked-In";
    }
}

//using System.ComponentModel.DataAnnotations;

//namespace Hotel_KYC_Api.Models
//{
//    public class GuestRegistration
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public string RoomNumber { get; set; }

//        [Required]
//        public string GuestName { get; set; }

//        public string CheckInTime { get; set; }
//        public string CheckOutTime { get; set; }

//        public int Adults { get; set; } = 1;
//        public int Kids { get; set; } = 0;

//        [Required]
//        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar must be 12 digits.")]
//        public string AadhaarNumber { get; set; }

//        public int Age { get; set; }

//        [Required]
//        public string MobileNumber { get; set; }

//        public string Address { get; set; }
//        public string ComingFrom { get; set; }
//        public string GoingTo { get; set; }

//        public DateTime CreatedAt { get; set; } = DateTime.Now;
//    }
//}