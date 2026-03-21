using Hotel_KYC_Api.Data;
using Hotel_KYC_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;

namespace Hotel_KYC_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GuestController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ REGISTER SINGLE GUEST
        [HttpPost("register-guest")]
        public async Task<IActionResult> RegisterGuest([FromBody] GuestRegistration guest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (guest.HotelId == 0)
                    guest.HotelId = 1;

                guest.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                _context.GuestRegistrations.Add(guest);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Guest registered successfully",
                    guestId = guest.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Database error",
                    details = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // 🚀🔥 CSV BULK UPLOAD
        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<GuestRegistration>().ToList();

                foreach (var record in records)
                {
                    // Default hotel
                    if (record.HotelId == 0)
                        record.HotelId = 1;

                    // UTC fix
                    record.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

                    // Basic duplicate check (optional)
                    bool exists = await _context.GuestRegistrations.AnyAsync(g =>
                        g.AadhaarNumber == record.AadhaarNumber &&
                        g.MobileNumber == record.MobileNumber);

                    if (!exists)
                        _context.GuestRegistrations.Add(record);
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "CSV uploaded successfully",
                    totalRecords = records.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "CSV upload failed",
                    details = ex.Message
                });
            }
        }

        // ✅ ALL GUESTS
        [HttpGet("all-guests")]
        public async Task<IActionResult> GetAllGuests()
        {
            try
            {
                var guests = await _context.GuestRegistrations
                    .Include(g => g.Hotel)
                    .OrderByDescending(g => g.CreatedAt)
                    .Select(g => new
                    {
                        id = g.Id,
                        guestName = g.GuestName,
                        roomNumber = g.RoomNumber,
                        mobileNumber = g.MobileNumber,
                        aadhaarNumber = g.AadhaarNumber,
                        adults = g.Adults,
                        kids = g.Kids,
                        createdAt = g.CreatedAt,
                        checkInTime = g.CheckInTime ?? "N/A",
                        checkOutTime = g.CheckOutTime ?? "Present",
                        hotelName = g.Hotel != null ? g.Hotel.HotelName : "N/A"
                    })
                    .ToListAsync();

                return Ok(guests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // 🚨 FLAGGED GUESTS
        [HttpGet("flagged-guests")]
        public async Task<IActionResult> GetFlaggedGuests()
        {
            var flaggedGuests = await _context.GuestRegistrations
                .Include(g => g.Hotel)
                .Where(g => g.IsFlagged || g.AadhaarNumber.Length < 12)
                .OrderByDescending(g => g.CreatedAt)
                .Select(g => new
                {
                    id = g.Id,
                    guestName = g.GuestName,
                    hotelName = g.Hotel != null ? g.Hotel.HotelName : "Unknown",
                    aadhaarNumber = g.AadhaarNumber,
                    roomNumber = g.RoomNumber,
                    mobileNumber = g.MobileNumber,
                    checkInTime = g.CheckInTime,
                    isFlagged = g.IsFlagged
                })
                .ToListAsync();

            return Ok(flaggedGuests);
        }

        // 🏨 ALL HOTELS
        [HttpGet("all-registered-hotels")]
        public async Task<IActionResult> GetAllRegisteredHotels()
        {
            var hotels = await _context.HotelRegistrations
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => new
                {
                    hotelId = h.Id,
                    hotelName = h.HotelName,
                    ownerName = h.OwnerName,
                    hotelAddress = h.Address,
                    mobileNumber = h.MobileNumber
                })
                .ToListAsync();

            return Ok(hotels);
        }

        // 🏨 HOTEL-WISE GUESTS
        [HttpGet("by-hotel/{hotelId}")]
        public async Task<IActionResult> GetGuestsByHotel(int hotelId)
        {
            var guests = await _context.GuestRegistrations
                .Where(g => g.HotelId == hotelId)
                .OrderByDescending(g => g.CreatedAt)
                .Select(g => new
                {
                    guestName = g.GuestName,
                    roomNumber = g.RoomNumber,
                    mobileNumber = g.MobileNumber,
                    createdAt = g.CreatedAt
                })
                .ToListAsync();

            return Ok(guests);
        }
    }
}