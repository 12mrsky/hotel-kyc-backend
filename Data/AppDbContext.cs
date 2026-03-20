using Hotel_KYC_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_KYC_Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<GuestRegistration> GuestRegistrations { get; set; }
        public DbSet<HotelRegistration> HotelRegistrations { get; set; }
    }
}
