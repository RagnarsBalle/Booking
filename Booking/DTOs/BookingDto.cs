namespace Booking.DTOs;

using System;

public class BookingDto
{
    public string CustomerName { get; set; }
    public int GuestID { get; set; }
    public int RoomID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public decimal TotalSum { get; set; }
}
