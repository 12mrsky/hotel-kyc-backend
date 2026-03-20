using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel_KYC_Api.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // 🔐 Hashed password (DB me ye hi save hoga)
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // 🔓 Plain password (sirf input ke liye, DB me store nahi hoga)
        [NotMapped]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string Role { get; set; } = "Guest";
    }
}