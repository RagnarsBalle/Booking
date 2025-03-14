using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booking.DTOs; // OBS! RoomModel bör importeras från Room-projektet

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

    // GET-testmetod
    [HttpGet("test")]
    public async Task<ActionResult<IEnumerable<string>>> GetTestBookings()
    {
        return Ok(new List<string> { "Test Booking 1", "Test Booking 2" });
    }

    // GET: api/Booking
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingModel>>> GetBookings()
    {
        return await _context.Bookings.ToListAsync();
    }

    // GET: api/Booking/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingModel>> GetBooking(int id)
    {
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.BookingID == id);

        if (booking == null)
            return NotFound();

        return booking;
    }

    // POST: api/Booking
    [HttpPost]
    public async Task<ActionResult<BookingModel>> CreateBooking([FromBody] BookingModel booking)
    {
        // Anropa RoomAPI via HttpClient
        var response = await _roomApiClient.GetAsync($"/api/room/{booking.RoomID}");


        if (!response.IsSuccessStatusCode)
            return BadRequest("Rummet hittades ej i Room-API.");

        // Konvertera svaret (JSON) till din lokala RoomDto
        var room = await response.Content.ReadFromJsonAsync<RoomDto>();

        if (room == null)
            return BadRequest("Rum existerar inte enligt Room-API.");

        if (!room.IsVacant)
            return BadRequest("Rum är upptaget enligt Room-API.");

        // Lägg till extra affärslogik om du behöver (pris, datumvalidering osv.)

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingID }, booking);
    }
}
