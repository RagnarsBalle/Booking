using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; } // Hashat lösenord

    [Required]
    public int RoleID { get; set; } // Referens till roll

    [ForeignKey("RoleID")]
    public Role Role { get; set; }

    public List<Session> Sessions { get; set; } = new List<Session>();
}