using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booking.DTOs;
using System.Net.Http.Json;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly BookingDbContext _context;
    private readonly HttpClient _roomApiClient;

    public BookingController(BookingDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _roomApiClient = httpClientFactory.CreateClient("RoomAPI");
    }

    // GET: /api/booking
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingModel>>> Get()
    {
        var bookings = await _context.Bookings.ToListAsync();
        return Ok(bookings);
    }

    // GET: /api/booking/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingModel>> GetBooking(int id)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingID == id);
        if (booking == null)
            return NotFound("Bokningen hittades inte.");

        return Ok(booking);
    }

    // POST: /api/booking
    [HttpPost]
    public async Task<ActionResult<BookingModel>> CreateBooking([FromBody] BookingDto bookingDto)
    {
        if (bookingDto == null)
        {
            Console.WriteLine("JSON-data är NULL! API:t kunde inte tolka den.");
            return BadRequest("Bokningsdata kan inte vara null.");
        }

        Console.WriteLine("JSON-data mottagen!");
        Console.WriteLine($"CustomerName: {bookingDto.CustomerName}");
        Console.WriteLine($"GuestID: {bookingDto.GuestID}");
        Console.WriteLine($"RoomID: {bookingDto.RoomID}");
        Console.WriteLine($"StartDate: {bookingDto.StartDate}");
        Console.WriteLine($"EndDate: {bookingDto.EndDate}");
        Console.WriteLine($"Adults: {bookingDto.Adults}");
        Console.WriteLine($"Children: {bookingDto.Children}");
        Console.WriteLine($"TotalSum: {bookingDto.TotalSum}");

        // 🟢 Skapa en ny bokning baserad på inkommande data
        var newBooking = new BookingModel
        {
            CustomerName = bookingDto.CustomerName,
            GuestID = bookingDto.GuestID,
            RoomID = bookingDto.RoomID,
            StartDate = bookingDto.StartDate,
            EndDate = bookingDto.EndDate,
            Adults = bookingDto.Adults,
            Children = bookingDto.Children,
            TotalSum = bookingDto.TotalSum
        };

        // 🟢 Lägg till i databaskontexten
        _context.Bookings.Add(newBooking);

        // 🟢 Spara ändringarna i databasen
        var result = await _context.SaveChangesAsync();
        Console.WriteLine($"Antal rader sparade i databasen: {result}");

        if (result > 0)
        {
            return CreatedAtAction(nameof(GetBooking), new { id = newBooking.BookingID }, newBooking);
        }
        else
        {
            return BadRequest("Bokningen kunde inte sparas.");
        }
    }



    // PUT: /api/booking/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDto bookingDto)
    {
        if (bookingDto == null)
            return BadRequest("Bokningsdata kan inte vara null.");

        var existingBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingID == id);
        if (existingBooking == null)
            return NotFound("Bokningen hittades inte.");

        existingBooking.GuestID = bookingDto.GuestID;
        existingBooking.RoomID = bookingDto.RoomID;
        existingBooking.StartDate = bookingDto.StartDate;
        existingBooking.EndDate = bookingDto.EndDate;
        existingBooking.Adults = bookingDto.Adults;
        existingBooking.Children = bookingDto.Children;
        existingBooking.TotalSum = bookingDto.TotalSum;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(existingBooking);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ett fel uppstod: {ex.Message}");
        }
    }

    // DELETE: /api/booking/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var existingBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingID == id);
        if (existingBooking == null)
            return NotFound("Bokningen hittades inte.");

        _context.Bookings.Remove(existingBooking);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
