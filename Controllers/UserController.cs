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

                var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    MissingFieldFound = null, // ignore missing columns
                    HeaderValidated = null   // ignore header validation
                };

                using var csv = new CsvReader(reader, config);

                var users = new List<User>();

                while (csv.Read())
                {
                    // 👉 skip empty rows
                    if (csv.Parser.Record.All(string.IsNullOrWhiteSpace))
                        continue;

                    var user = new User
                    {
                        // 👉 If DB auto-increment use karta hai, ye line hata sakte ho
                        UserId = csv.GetField<int>(0),

                        FullName = csv.GetField(1)?.Trim() ?? "",
                        Email = csv.GetField(2)?.Trim() ?? "",
                        PasswordHash = csv.GetField(3)?.Trim() ?? "",
                        PhoneNumber = csv.GetField(4)?.Trim() ?? "",

                        CreatedAt = DateTime.TryParse(csv.GetField(5), out var dt)
                            ? DateTime.SpecifyKind(dt, DateTimeKind.Utc)
                            : DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),

                        Role = csv.GetField(6)?.Trim() ?? "User"
                    };

                    users.Add(user);
                }

                if (users.Count == 0)
                    return BadRequest("CSV file is empty or invalid");

                await _context.Users.AddRangeAsync(users);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Users uploaded successfully",
                    count = users.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "CSV upload failed",
                    details = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}