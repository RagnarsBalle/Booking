namespace Booking.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookingModel
{
    [Key]
    public int BookingID { get; set; }

    public int GuestID { get; set; }

    public int RoomID { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int Adults { get; set; }
    public int Children { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalSum { get; set; }
}
