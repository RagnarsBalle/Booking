
using Booking.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Session
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [Required]
    public string SessionToken { get; set; } // Unikt sessions-ID

    [Required]
    public DateTime ExpirationTime { get; set; } // När sessionen löper ut
}