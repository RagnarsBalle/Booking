namespace Booking.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class BookingModel
    {
        public int BookingID { get; set; }
        public int GuestID { get; set; }
        public string RoomID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public Decimal TotalSum { get; set; }
    }
}
