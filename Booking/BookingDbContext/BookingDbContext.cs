using System.Collections.Generic;

namespace Booking.BookingDbContext
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<YourDbContext> options) : base(options) { }

        public DbSet<YourEntity> YourEntities { get; set; }
    }
}
