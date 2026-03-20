using System.ComponentModel.DataAnnotations;

namespace Hotel_KYC_Api.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        // 🔥 ADD THIS (for incoming request only)
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🔥 DEFAULT ROLE
        public string Role { get; set; } = "Guest";
    }

    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}