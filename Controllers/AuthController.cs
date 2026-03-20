using Hotel_KYC_Api.Data;
using Hotel_KYC_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_KYC_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ================= REGISTER =================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null ||
                string.IsNullOrEmpty(request.FullName) ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Password) ||
                string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            var existingUser = await _context.Users
                .AnyAsync(u => u.Email == request.Email);

            if (existingUser)
            {
                return BadRequest(new { message = "Email is already registered." });
            }

            try
            {
                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordHash = request.Password,

                    PhoneNumber = request.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    Role = "User"

                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "User registered successfully",
                    userId = user.UserId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Database error",
                    error = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        // ================= LOGIN =================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            Console.WriteLine("EMAIL: " + request.Email);
            Console.WriteLine("PASSWORD: " + request.Password);

            if (request == null ||
                string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and Password required" });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            // ✅ SIMPLE PASSWORD MATCH
            if (request.Password != user.PasswordHash)
            {
                return Unauthorized(new { message = "Invalid Username or Password" });
            }

            return Ok(new
            {
                message = "Login successful",
                userId = user.UserId,
                fullName = user.FullName,
                email = user.Email,
                role = user.Role
            });
        }

        // ================= LOGIN DTO =================
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}