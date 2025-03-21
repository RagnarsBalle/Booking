using Microsoft.EntityFrameworkCore;
using Booking.Models;

namespace Booking.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

        // Lägg till endast de entiteter du har
        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<ApiUsageLog> ApiUsageLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurera primärnyckel
            modelBuilder.Entity<BookingModel>()
                .HasKey(b => b.BookingID); // Primärnyckel för BookingModel

            // Om du vill lägga till relationer, lägg till dem här
            modelBuilder.Entity<BookingModel>()
                .Property(b => b.TotalSum)
                .HasColumnType("decimal(18,2)"); // Rätt format i databasen
        }
    }
}
