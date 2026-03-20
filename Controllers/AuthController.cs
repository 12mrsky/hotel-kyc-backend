using Hotel_KYC_Api.Data;
using Hotel_KYC_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_KYC_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Invalid data");

                if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                    return BadRequest("Email and Password are required");

                // ✅ Check duplicate email
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null)
                    return BadRequest("User already exists");

                // 🔐 Hash password (IMPORTANT FIX)
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // 🕒 Set created date
                user.CreatedAt = DateTime.UtcNow;

                // 👤 Default role
                if (string.IsNullOrEmpty(user.Role))
                    user.Role = "Guest";

                // ❌ Password plain save nahi karna
                user.Password = null;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "User registered successfully",
                    user.Email,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                // 🔍 Detailed error
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == login.Email);

                if (user == null)
                    return Unauthorized("Invalid email or password");

                // 🔐 Verify password
                bool isValid = BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);

                if (!isValid)
                    return Unauthorized("Invalid email or password");

                return Ok(new
                {
                    message = "Login successful",
                    user.Email,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }

    // ✅ LOGIN DTO (clean request model)
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}