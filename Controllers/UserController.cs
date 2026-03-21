using Hotel_KYC_Api.Data;
using Hotel_KYC_Api.Models;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;

namespace Hotel_KYC_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var users = new List<User>();

                while (csv.Read())
                {
                var user = new User
{
    UserId = csv.GetField<int>(0),

    FullName = csv.GetField(1) ?? "",
    Email = csv.GetField(2) ?? "",
    PasswordHash = csv.GetField(3) ?? "",
    PhoneNumber = csv.GetField(4) ?? "",

    CreatedAt = DateTime.TryParse(csv.GetField(5), out var dt)
        ? dt
        : DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),

    Role = csv.GetField(6) ?? "User"
};
                }

                _context.Users.AddRange(users);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Users uploaded", count = users.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}