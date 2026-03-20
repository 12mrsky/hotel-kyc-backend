using System.ComponentModel.DataAnnotations;

namespace Hotel_KYC_Api.Models
{
   
        public class User
        {
            public int UserId { get; set; }   // Identity column, auto-generated
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public string Role { get; set; }
    }



    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }

}
