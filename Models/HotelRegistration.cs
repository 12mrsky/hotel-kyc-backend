using System.ComponentModel.DataAnnotations;

namespace Hotel_KYC_Api.Models
{
    public class HotelRegistration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HotelName { get; set; }

        [Required]
        public string OwnerName { get; set; }

        public string GSTNumber { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}