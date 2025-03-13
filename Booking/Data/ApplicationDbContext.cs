using Booking.Models;
using Microsoft.EntityFrameworkCore;
using Room.Models;
using System.Collections.Generic;

namespace Room.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Lägg till en parameterlös konstruktör för design-time-migrationer
        public ApplicationDbContext() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}