﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Booking.BookingDbContext
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

        public DbSet<BookingDbContext> YourEntities { get; set; }
    }
}
