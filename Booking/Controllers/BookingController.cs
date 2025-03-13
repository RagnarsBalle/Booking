using Booking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly BookingDbContext _context;

    public BookingController(BookingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> Get()
    {
        return Ok(new List<string> { "Test Booking 1", "Test Booking 2" });
    }

    [HttpPost]
    public async Task<ActionResult<string>> Post()
    {
        return Ok("Booking created successfully.");
    }
}